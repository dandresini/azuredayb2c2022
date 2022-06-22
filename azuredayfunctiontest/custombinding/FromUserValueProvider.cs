using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace azuredayfunctiontest.custombinding
{
    public class FromUserValueProvider : IValueProvider
    {
        private ILogger<Startup> logger;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public string AutorizedScopes{ get; set; }

        public FromUserValueProvider(ILogger<Startup> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            _HttpContextAccessor = httpContextAccessor;
        }

        public Task<object> GetValueAsync()
        {
            logger.LogInformation($"Inizio procedura di controllo");
            bool isAuth = _HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            logger.LogInformation($"Utente autorizzato {isAuth}");
            if (isAuth)
            {
                var claims = _HttpContextAccessor.HttpContext.User.Claims;

                if (AutorizedScopes is not null)
                {
                    logger.LogInformation($"Verifico scope {AutorizedScopes}");
                    isAuth = claims.Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/scope" && c.Value.Split(",").Contains(AutorizedScopes)).Any();
                    logger.LogInformation($"Scope verificato {isAuth}");
                }                    
                
                if (isAuth)
                    return Task.FromResult<object>(new MyUserModel
                    {
                        Name = claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").FirstOrDefault()?.Value,
                        Surname = claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").FirstOrDefault()?.Value,
                        City = claims.Where(c => c.Type == "city").FirstOrDefault()?.Value,
                        Email = claims.Where(c => c.Type == "emails").FirstOrDefault()?.Value,
                        IsAuthenticated = isAuth
                    }); 
            }
            
            return Task.FromResult<object>(new MyUserModel{IsAuthenticated = false});
            
        }

        public Type Type => typeof(MyUserModel);

        public string ToInvokeString() => String.Empty;
    }
}
