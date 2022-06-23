# Proteggere una Function con Azure Active Directory B2C
Il presente progetto spiega come proteggere le **Azure Function** con **Azure Active Directory B2C**
Prepariamo il Tenant configurando:
1. Un applicazione di tipo Web che chiameremo **Azure Function** senza alcun URI di reindirizzamento dovè esporremo il nostro ambito di utilizzo (function.read);
2. Un applicazione di tipo Web che chiameremo **Test Api** con reindirizzamento al portale [jwt.ms](https://jwt.ms) che ci permetterà di testare la nostra Azure Function;
3. Un applicazione di tipo SPA (single page application) che chiameremo **App React** con reindirizzamento alla nostra app di test in react in locale [http://localhost:3000](http://localhost:3000);

Per gli ultimi 2 punti sopra elencati ricordarsi:
1. nella sezione "Authentication" dell'App registrata di verificate di avere selezionato quali token rilasciare (nel nostro caso spuntare le voci Access Tokens e ID Tokens);
2. nella sezione "Api Permission" dell'App registrata di verificare di aver fornito il consenso ammministrativo per l'ambito (function.read);

Ultimata la configurazione del Tenant con la configurazione degli User Flows passiamo alla creazione della nostra Azure Function, ricordandosi di utilizzare come SO Windows così da poter scrivere il codice della function direttamente sul portale di Azure.

Inserire il seguente codice per testare la function dal portale di Azure
```
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;


[FunctionName("UserInformationSimple")]
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

Scaricare il repository per creare un progetto react + msal:
[msal react project](https://github.com/Azure-Samples/ms-identity-javascript-react-spa)

Link per visualizzare il problema noto del non funzionamento della "RequiredScope" in una Azure Function
[RequiredScope not work](https://github.com/AzureAD/microsoft-identity-web/issues/1002)