using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SecuroteckWebApplication.Models;

namespace SecuroteckWebApplication.Controllers
{
    public class APIAuthorisationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IEnumerable<string> key;
            request.Headers.TryGetValues("ApiKey", out key);
            if(key !=null)
            {
                try
                {
                    Guid keyValid = new Guid(key.FirstOrDefault());
                }
                catch (Exception e) //Can be used to log if needed
                {
                    return base.SendAsync(request, cancellationToken); //Don't send exception, user shouldn't get the error info.
                }
                UserDatabaseAccess dbUser = new UserDatabaseAccess();
                if (dbUser.DoesUserExist(key.First()))
                {
                    User user = dbUser.getUserIfExists(key.First());
                    Claim claim = new Claim("name",user.UserName);
                    Claim[] claimants = new Claim[1]{claim};
                    ClaimsIdentity identity = new ClaimsIdentity(claimants, "ApiKey");
                    ClaimsPrincipal principle = new ClaimsPrincipal(identity);
                    Thread.CurrentPrincipal = principle;
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}