using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SecuroteckWebApplication.Controllers
{
    public class UserController : ApiController
    {
        [ActionName("New")]
        public string Get([FromUri]string username)
        {
            Models.UserDatabaseAccess dbUser = new Models.UserDatabaseAccess();
            if (dbUser.isUsernameUsed(username) && username != null && username != string.Empty)
            {
                return "True - User Does Exist! Did you mean to do a POST to create a new user?";
            }
            return "False - User Does Not Exist! Did you mean to do a POST to create a new user?";
        }
        [ActionName("New")]
        public IHttpActionResult Post([FromBody]string username)
        {
            HttpRequestMessage r = Request;
            Models.UserDatabaseAccess dbUser = new Models.UserDatabaseAccess();
            if (username != null && username != string.Empty)
            {
                return Ok(dbUser.createUser(username));
            }
            return BadRequest("Oops.Make sure your body contains a string with your username and your Content - Type is Content - Type:application / json");
        }
        [CustomAuthorise]
        [ActionName("RemoveUser")]
        public IHttpActionResult Delete([FromUri]string username)
        {
            IEnumerable<string> key;
            Request.Headers.TryGetValues("ApiKey", out key);
            Models.UserDatabaseAccess dbUser = new Models.UserDatabaseAccess();
            if (username == dbUser.getUserIfExists(key.First()).UserName)
            {
                try
                {
                    log(message: "User/Delete - Pass", apiKey: key.First());
                }
                catch (Exception e)
                {
                    if (username == dbUser.getUserIfExists(key.First()).UserName)
                        return Ok(false.ToString()); 
                    else
                        return Ok(dbUser.deleteUser(key.First()).ToString());

                }
                return Ok(dbUser.deleteUser(key.First()).ToString());
            }
            log(message: "User/Delete - Fail", apiKey: key.First());
            return Ok(false.ToString());
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
