using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace SecuroteckClient
{
    class Client
    {
        static string HOST = "http://localhost:";
        static string PORT = "24702";
        static bool exit = false;
        static string lastApiKey = null;
        static string lastUsername = null;
        static string publicRSA = null;
        static void Main(string[] args)
        {
            try
            {
                Startup();
                UserInputManager();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            while (!exit)
            {
                try
                {
                    UserInputManager();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void Startup()
        {
            Console.WriteLine("Hello. What would you like to do?");
        }
        static async void UserInputManager()
        {
            string command = Console.ReadLine();
            Console.Clear();
            string[] commandFirstSplit = command.Split(separator: new char[] { ' ' }, count: 2);
            string commandType = commandFirstSplit[0];
            commandType = commandType.ToUpper();

            switch (commandType)
            {
                case "TALKBACK":
                    await TalkbackManager(commandFirstSplit);
                    break;
                case "USER":
                    await UserManager(commandFirstSplit);
                    break;
                case "PROTECTED":
                    await ProtectedManager(commandFirstSplit[1]);
                    break;
                case "EXIT":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Command type not recognised.");
                    Console.WriteLine("Please use Talkback, User, or Protected");
                    break;
            }
            Console.WriteLine("What would you like to do next?");
        }
        static async Task<string> TalkbackManager(string[] args)
        {
            string response = "";
            //Args 0 is the command type
            Console.WriteLine("...please wait...");
            string[] commands = args[1].Split(' ');
            string temp = commands[0].ToUpper();
            try
            {
                switch (temp)
                {
                    case "SORT":
                        if (commands.Length > 1)
                        {
                            commands[1] = commands[1].Remove(commands[1].Length - 1);
                            commands[1] = commands[1].Replace('[', ' ');
                            commands[1] = commands[1].Trim();
                            string[] ints = commands[1].Split(new char[] { ',' });
                            response = await TalkbackSort(ints);
                        }
                        else
                        {
                            Console.WriteLine("Please enter integers to sort.");
                            Console.WriteLine("Ex: [5,4,3,2,1]");
                        }
                        break;
                    case "HELLO":
                        response = await TalkbackHello();
                        break;
                    default:
                        Console.WriteLine("Talkback command not recognised");
                        Console.WriteLine("Comands are Hello, and Sort");
                        break;
                }
            }
            catch (ArgumentNullException nullEx)
            {
                Console.WriteLine("Talkback command not recognised");
                Console.WriteLine(nullEx.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return response;
        }
        static async Task<string> TalkbackSort(string[] toSort)
        {
            StringBuilder sb = new StringBuilder();
            string responseContent = "";
            sb.Append(HOST + PORT + "/api/talkback/sort?");
            foreach (string s in toSort)
            {
                sb.Append("integers=" + s + "&");
            }
            sb.Remove(sb.Length - 1, 1);
            if (toSort.Length == 0)
            {
                sb.Clear();
                sb.Append(HOST + PORT + "/api/talkback/sort?");
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage msg = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(sb.ToString())
            };
            msg.Headers.Add("Accept", "application/json");
            var response = await client.SendAsync(msg);
            if (response.IsSuccessStatusCode)
                responseContent = await response.Content.ReadAsStringAsync();
            else
                responseContent = response.StatusCode.ToString() + "\r\n" + await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            return responseContent;
        }
        static async Task<string> TalkbackHello()
        {
            StringBuilder sb = new StringBuilder();
            string responseContent = "";
            sb.Append(HOST + PORT + "/api/talkback/hello");
            HttpClient client = new HttpClient();
            HttpRequestMessage msg = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(sb.ToString())
            };
            msg.Headers.Add("Accept", "application/json");
            var response = await client.SendAsync(msg);
            responseContent = await ResponseReader(response);
            Console.WriteLine(responseContent);
            return responseContent;
        }
        static async Task UserManager(string[] args)
        {
            //args 0 is command type
            string[] UserCommand = args[1].Split(new char[] { ' ' }, 2);
            string temp = UserCommand[0].ToUpper();
            try
            {
                try
                {
                    switch (temp)
                    {
                        case "GET":
                            await GetUser(UserCommand[1]);
                            break;
                        case "POST":
                            await PostUser(UserCommand[1]);
                            break;
                        case "SET":
                            if (SetUser(UserCommand[1]))
                                Console.WriteLine("Stored");
                            else
                                Console.WriteLine("Invalid User Set command");
                            break;
                        case "DELETE":
                            await DeleteUser();
                            break;
                        default:
                            Console.WriteLine("User command not recognised");
                            Console.WriteLine("Commands are Get, Post, Set, and Delete");
                            break;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("Please enter values after the command");
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("User command not recognised");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static async Task GetUser(string user)
        {
            StringBuilder sb = new StringBuilder();
            string responseContent = "";
            sb.Append(HOST + PORT + "/api/user/new?username=" + user);
            HttpClient client = new HttpClient();
            HttpRequestMessage msg = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(sb.ToString())
            };
            msg.Headers.Add("Accept", "application/json");
            var response = await client.SendAsync(msg);
            responseContent = await ResponseReader(response);
            Console.WriteLine(responseContent);
        }
        static async Task PostUser(string user)
        {
            StringBuilder sb = new StringBuilder();
            string responseContent = "";
            sb.Append(HOST + PORT + "/api/user/new");
            HttpClient client = new HttpClient();
            var postBody = new StringContent("\"" + user + "\"", Encoding.UTF8, "application/json");
            var response = await client.PostAsync(sb.ToString(), postBody);
            if (response.IsSuccessStatusCode)
            {
                responseContent = "Got API Key";
                lastApiKey = JsonConvert.DeserializeObject<string>(await (response.Content.ReadAsStringAsync()));
                lastUsername = user;
            }
            else
                responseContent = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            Console.WriteLine(responseContent);
        }
        static bool SetUser(string args)
        {
            string[] argsSplit = args.Split(' ');
            if (argsSplit.Length == 2)
            {
                lastUsername = argsSplit[0];
                lastApiKey = argsSplit[1];
                return true;
            }
            else
            {
                Console.WriteLine("Please enter a username and Api Key seperated by a space");
                return false;
            }
        }
        static async Task<string> DeleteUser()
        {
            string returnString = "";
            if (!string.IsNullOrEmpty(lastApiKey) && !string.IsNullOrEmpty(lastUsername))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(HOST + PORT + "/api/user/removeuser?username=" + lastUsername);
                HttpClient client = new HttpClient();
                HttpRequestMessage msg = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(sb.ToString())
                };
                msg.Headers.Add("ApiKey", lastApiKey);
                msg.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(msg);
                returnString = await ResponseReader(response);
                Console.WriteLine(returnString);
            }
            else
            {
                returnString = "You need to do a User Post or User Set first";
            }
            return returnString;
        }
        static async Task ProtectedManager(string args)
        {
            string[] command = args.Split(new char[] { ' ' }, 2);
            try
            {
                switch (command[0].ToUpper())
                {
                    case "HELLO":
                        await ProtectedHello();
                        break;
                    case "SHA1":
                        await ProtectedSha1(command[1]);
                        break;
                    case "SHA256":
                        await ProtectedSha256(command[1]);
                        break;
                    case "GET":
                        await ProtectedGet();
                        break;
                    case "SIGN":
                        await ProtectedSign(command[1]);
                        break;
                    default:
                        Console.WriteLine("Invalid Protected command");
                        break;
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Invalid Protected command");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static async Task ProtectedHello()
        {
            string returnString;
            if (HasKey())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(HOST + PORT + "/api/protected/hello");
                HttpClient client = new HttpClient();
                HttpRequestMessage msg = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(sb.ToString())
                };
                msg.Headers.Add("ApiKey", lastApiKey);
                msg.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(msg);
                returnString = await ResponseReader(response);
                Console.WriteLine(returnString);
            }
        }
        static async Task ProtectedGet()
        {
            string returnString;
            if (HasKey())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(HOST + PORT + "/api/protected/getpublickey");
                HttpClient client = new HttpClient();
                HttpRequestMessage msg = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(sb.ToString())
                };
                msg.Headers.Add("ApiKey", lastApiKey);
                msg.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(msg);
                returnString = await ResponseReader(response);
                if (response.IsSuccessStatusCode)
                {
                    publicRSA = returnString;
                    Console.WriteLine("Got Public Key");
                }
                else
                    Console.WriteLine("Couldn’t Get the Public Key");
            }
        }
        static async Task ProtectedSign(string message)
        {
            string returnString;
            if (HasKey() && !string.IsNullOrEmpty(publicRSA))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(HOST + PORT + "/api/protected/sign?message=" + message);
                HttpClient client = new HttpClient();
                HttpRequestMessage msg = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(sb.ToString())
                };
                msg.Headers.Add("ApiKey", lastApiKey);
                msg.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(msg);
                returnString = await ResponseReader(response);

                //Set public key
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(publicRSA);
                byte[] messageArray = Encoding.UTF8.GetBytes(message);
                byte[] signedArray = StringToByteArray(returnString);

                if (rsa.VerifyData(messageArray, CryptoConfig.MapNameToOID("SHA1"), signedArray))
                    Console.WriteLine("Message was successfully signed");
                else
                    Console.WriteLine("Message was not successfully signed");
            }
            if (string.IsNullOrEmpty(publicRSA))
                Console.WriteLine("Client doesn’t yet have the public key");
        }
        public static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace("-", "");

            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
        static async Task ProtectedSha1(string toEncrypt)
        {
            string returnString;
            if (HasKey())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(HOST + PORT + "/api/protected/sha1?message=" + toEncrypt);
                HttpClient client = new HttpClient();
                HttpRequestMessage msg = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(sb.ToString())
                };
                msg.Headers.Add("ApiKey", lastApiKey);
                msg.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(msg);
                returnString = await ResponseReader(response);
                Console.WriteLine(returnString);
            }
        }
        static async Task ProtectedSha256(string toEncrypt)
        {
            string returnString;
            if (HasKey())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(HOST + PORT + "/api/protected/sha256?message=" + toEncrypt);
                HttpClient client = new HttpClient();
                HttpRequestMessage msg = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(sb.ToString())
                };
                msg.Headers.Add("ApiKey", lastApiKey);
                msg.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(msg);
                returnString = await ResponseReader(response);
                Console.WriteLine(returnString);
            }
        }
        static async Task<string> ResponseReader(HttpResponseMessage response)
        {
            try
            {
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                else
                {
                    MessageObj msg = JsonConvert.DeserializeObject<MessageObj>(await response.Content.ReadAsStringAsync());
                    return response.StatusCode.ToString() + "\r\n" + msg.Message;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading JSON");
                Console.WriteLine(e.Message);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                    return response.StatusCode.ToString() + "\r\n" + await response.Content.ReadAsStringAsync();
            }
        }
        static bool HasKey()
        {
            bool key = false;
            key = !string.IsNullOrEmpty(lastApiKey);
            if (!key)
                Console.WriteLine("You need to do a User Post or User Set first");
            return key;
        }
    }
}
