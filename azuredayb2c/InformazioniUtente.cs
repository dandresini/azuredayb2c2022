using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//inserimento namespace per scope

using azuredayb2c.custombinding;
using Microsoft.Identity.Web.Resource;

namespace azuredayb2c
{
    public static class InformazioniUtente
    {
        [FunctionName("InformazioniUtente")]
        //[RequiredScope(new string[] {"function.read"})]
        public static  Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [FromUser(AutorizedScopes = "function.read",ClientId = "1ac83858-77dd-489a-ac6e-9e8e445c0f23")]  MyUserModel UserInformation,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            if (UserInformation.IsAuthenticated)
            {
                //req.HttpContext.User.Claims
                
                return Task.FromResult<IActionResult>(new OkObjectResult(UserInformation));
            }
            
            return Task.FromResult<IActionResult>(new ObjectResult("Forbidden") { StatusCode = 403 });
        }
    }
}
