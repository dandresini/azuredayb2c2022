using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using azuredayfunctiontest.custombinding;

namespace azuredayfunctiontest
{
    public static class UserInformation
    {
        [FunctionName("UserInformation")]
        public static  Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [FromUser(AutorizedScopes = "function.read")] MyUserModel UserInformation,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (UserInformation.IsAuthenticated)
                return Task.FromResult<IActionResult>(new OkObjectResult(UserInformation));
            

            return Task.FromResult<IActionResult>(new ObjectResult("Forbidden") { StatusCode = 403 });
        }
    }
}
