using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Http;
using SecuroteckWebApplication.Controllers;

namespace SecuroteckWebApplication
{    
    public static class WebApiConfig
    {
        public static RSACryptoServiceProvider RSAKey;
        static CspParameters CSP = new CspParameters() { KeyContainerName = "PublicPrivateKey", Flags = CspProviderFlags.UseMachineKeyStore};        
        // Publically accessible global static variables could go here

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            GlobalConfiguration.Configuration.MessageHandlers.Add(new APIAuthorisationHandler());

            #region Task 7
            // Configuration for Task 9
            RSAKey = new RSACryptoServiceProvider(CSP);
            
            #endregion

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "TalkbackApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
