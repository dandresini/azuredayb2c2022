# Proteggere una Function con Azure Active Directory B2C
Il presente progetto permette di proteggere le Azure Function tramite Azure Active Directory **B2C**
Prima di iniziare bisogna preparare il tenant configurando i flussi utenti e registrando le applicazioni.

una volta ultimata la procedura possiamo scaricare il seguente repository in react + msal 
[msal react project](https://github.com/Azure-Samples/ms-identity-javascript-react-spa)

Inserire il seguente codice per testare la function dal portale di Azure
```
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;


[FunctionName("Informazioni")]
public static Task<IActionResult> Run(
    HttpRequest req,
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
```

[RequiredScope not work](https://github.com/AzureAD/microsoft-identity-web/issues/1002)