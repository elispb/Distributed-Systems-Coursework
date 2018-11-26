using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;

namespace SecuroteckWebApplication.Controllers
{
    public class ProtectedController : ApiController
    {
        [ActionName("Hello")]
        [CustomAuthorise]
        [HttpGet]
        public IHttpActionResult GetHello()
        {
            IEnumerable<string> key;
            Request.Headers.TryGetValues("ApiKey", out key);
            Models.UserDatabaseAccess dbUser = new Models.UserDatabaseAccess();
            string response = "Hello " + dbUser.getUserIfExists(key.First()).UserName;
            log(message: "Protected/Hello - Pass", apiKey: key.First());
            return Ok(response);
        }

        [ActionName("Sha1")]
        [CustomAuthorise]
        [HttpGet]
        public IHttpActionResult GetSha1([FromUri] string message)
        {
            IEnumerable<string> key;
            Request.Headers.TryGetValues("ApiKey", out key);
            if (!string.IsNullOrEmpty(message))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                SHA1 hash = SHA1.Create();
                byte[] hashedBytes = hash.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    var hex = b.ToString("X2");
                    sb.Append(hex);
                }
                log(message: "Protected/Sha1 - Pass", apiKey: key.First());
                return Ok(sb.ToString());
            }
            log(message: "Protected/Sha1 - Fail", apiKey: key.First());
            return BadRequest("Bad Request");
        }

        [ActionName("Sha256")]
        [CustomAuthorise]
        [HttpGet]
        public IHttpActionResult GetSha256([FromUri] string message)
        {
            IEnumerable<string> key;
            Request.Headers.TryGetValues("ApiKey", out key);
            if (!string.IsNullOrEmpty(message))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                SHA256 hash = SHA256.Create();
                byte[] hashedBytes = hash.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    var hex = b.ToString("X2");
                    sb.Append(hex);
                }
                log(message: "Protected/Sha256 - Pass", apiKey: key.First());
                return Ok(sb.ToString());
            }
            log(message: "Protected/Sha256 - Fail", apiKey: key.First());
            return BadRequest("Bad Request");
        }

        [ActionName("GetPublicKey")]
        [CustomAuthorise]
        [HttpGet]
        public IHttpActionResult GetPublicKey()
        {
            IEnumerable<string> key;
            Request.Headers.TryGetValues("ApiKey", out key);
            log(message: "Protected/GetPublicKey - Pass", apiKey: key.First());
            return Ok(WebApiConfig.RSAKey.ToXmlString(false));
        }

        [ActionName("Sign")]
        [CustomAuthorise]
        [HttpGet]
        public IHttpActionResult GetSign([FromUri] string message)
        {
            IEnumerable<string> key;
            Request.Headers.TryGetValues("ApiKey", out key);
            log(message: "Protected/Sign - Pass", apiKey: key.First());
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            var hexSigned = BitConverter.ToString(WebApiConfig.RSAKey.SignData(bytes, "SHA1"));
            return Ok(hexSigned);
        }

        public bool log(string message, string apiKey)
        {
            Models.UserDatabaseAccess dbUser = new Models.UserDatabaseAccess();
            Models.LogDatabaseAccess dbLog = new Models.LogDatabaseAccess();
            dbUser.saveLog(dbLog.createLog(message), apiKey);
            return false;
        }
    }
}
