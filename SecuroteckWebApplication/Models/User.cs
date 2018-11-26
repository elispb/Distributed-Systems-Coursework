using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml;

namespace SecuroteckWebApplication.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid APIKey { get; set; }
        public string UserName { get; set; }
        public ICollection<Log> logs { get; set; }
        public bool active { get; set; }
        public User(bool status)
        {
            active = status;
            logs = new List<Log>();
        }
        public User()
        {
            active = true;
            logs = new List<Log>();
        }

    }
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int logId { get; set; }
        public string LogMessage { get; set; }
        public DateTime ActionDateTime { get; set; }
        public Log() { }
        public Log(string message)
        {
            LogMessage = message;
            ActionDateTime = DateTime.Now;
        }

    }

    //public class ArchivedLog
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int ArchivedLogId { get; set; }
    //    public string LogMessage { get; set; }
    //    public DateTime ActionDateTime { get; set; }
    //    public DateTime ArchivedDateTime { get; set; }
    //    public string ArchivedNote { get; set; }
    //    public string UserApiKey { get; set; }

    //    public ArchivedLog(Log OldLog, string ApiKey)
    //    {
    //        LogMessage = OldLog.LogMessage;
    //        ActionDateTime = OldLog.ActionDateTime;
    //        ArchivedDateTime = DateTime.Now;
    //        UserApiKey = ApiKey;
    //    }
    //    public ArchivedLog(Log OldLog, string ApiKey, string ArchiveReason)
    //    {
    //        LogMessage = OldLog.LogMessage;
    //        ActionDateTime = OldLog.ActionDateTime;
    //        ArchivedDateTime = DateTime.Now;
    //        ArchivedNote = ArchiveReason;
    //        UserApiKey = ApiKey;
    //    }
    //}

    public class UserDatabaseAccess
    {
        public string createUser(string username)
        {
            using (var ctx = new UserContext())
            {
                User newUser = new User() { UserName = username };
                ctx.Users.Add(newUser);
                ctx.SaveChanges();
                return newUser.APIKey.ToString();
            }
        }
        public bool saveLog(Log toSave, string apiKey)
        {
            Guid key = new Guid(apiKey);
            using (var ctx = new UserContext())
            {
                if (ctx.Users.Any(o => o.APIKey == key))
                {
                    var user = ctx.Users.Find(key);
                    user.logs.Add(toSave);
                    ctx.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool isUsernameUsed(string username)
        {
            using (var ctx = new UserContext())
            {
                return (ctx.Users.Any(o => o.UserName == username && o.active == true));
            }
        }
        public bool DoesUserExist(string apiKey)
        {
            Guid key = new Guid(apiKey);
            using (var ctx = new UserContext())
            {
                return (ctx.Users.Any(o => o.APIKey == key && o.active == true));
            }
        }
        public bool DoesUserExist(string apiKey, string username)
        {
            Guid key = new Guid(apiKey);
            using (var ctx = new UserContext())
            {
                return (ctx.Users.Any(o => o.APIKey == key && o.UserName == username && o.active == true)) ;
            }
        }
        public User getUserIfExists(string apiKey)
        {
            Guid key = new Guid(apiKey);
            using (var ctx = new UserContext())
            {
                if (ctx.Users.Any(o => o.APIKey == key))
                {
                    var user = ctx.Users.Find(key);
                    if (user.active)
                    {
                        return user;
                    }
                }
                return null;
            }
        }
        public bool deleteUser(string apiKey)
        {
            Guid key = new Guid(apiKey);
            using (var ctx = new UserContext())
            {
                if (ctx.Users.Any(o => o.APIKey == key && o.active == true))
                {
                    var user = ctx.Users.Find(key);
                    user.active = false;
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }

    public class LogDatabaseAccess
    {
        public Log createLog(string message)
        {
            using (var ctx = new UserContext())
            {
                Log newLog = new Log(message);
                return newLog;
            }
        }
    }


}