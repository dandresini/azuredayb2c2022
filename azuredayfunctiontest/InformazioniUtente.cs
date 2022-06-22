using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace azuredayfunctiontest
{
    public static class InformazioniUtente
    {
        [FunctionName("InformazioniUtente")]
        public static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var User = req.HttpContext.User;
            if (User.Identity.IsAuthenticated)
            {
                log.LogInformation("Utente Autenticato");
                var claims = User.Claims;

                var returnValue = new
                {
                    Name = claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").FirstOrDefault()?.Value,
                    Surname = claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").FirstOrDefault()?.Value,
                    City = claims.Where(c => c.Type == "city").FirstOrDefault()?.Value,
                    Email = claims.Where(c => c.Type == "emails").FirstOrDefault()?.Value
                };

                return Task.FromResult<IActionResult>(new ObjectResult(returnValue));
            }
            log.LogInformation("Utente non Autenticato");
            return Task.FromResult<IActionResult>(new ObjectResult("Forbidden") { StatusCode = 403 });
        }
    }
}
