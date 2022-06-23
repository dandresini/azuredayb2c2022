# Proteggere una Function con Azure Active Directory B2C
Il presente progetto spiega come proteggere le **Azure Function** con **Azure Active Directory B2C**.<br />
Prepariamo il Tenant configurando:
1. Un applicazione di tipo Web che chiameremo **Azure Function** senza alcun URI di reindirizzamento dovè esporremo il nostro ambito di utilizzo (function.read);
<br />
<img width="836" alt="app_azure_function" src="https://user-images.githubusercontent.com/3338265/175345163-1b4f0437-b369-40a2-b630-2fbddf9eac54.png">
<br />
<img width="836" alt="ambito" src="https://user-images.githubusercontent.com/3338265/175345818-57a6fa4b-d34f-48a6-a109-b5b89cf896f8.png">
<br />
2. Un applicazione di tipo Web che chiameremo **Test Function** con reindirizzamento al portale [jwt.ms](https://jwt.ms) che ci permetterà di testare la nostra Azure Function;
<br />
<img width="836" alt="Test Function" src="https://user-images.githubusercontent.com/3338265/175346458-200656ca-aba1-422c-9b8a-b60311071e15.png">
<br />
3. Un applicazione di tipo SPA (single page application) che chiameremo **React Appt** con reindirizzamento alla nostra app di test in react in locale [http://localhost:3000](http://localhost:3000);
<br />
<img width="835" alt="reactapp" src="https://user-images.githubusercontent.com/3338265/175348105-1763a762-51ad-440c-830e-eb463ad704bd.png">
<br /><br />
Per gli ultimi 2 punti sopra elencati ricordarsi:
1. nella sezione "Authentication" dell'App registrata di verificate di avere selezionato quali token rilasciare (nel nostro caso spuntare le voci Access Tokens e ID Tokens);
<br />
<img width="830" alt="token" src="https://user-images.githubusercontent.com/3338265/175347564-40ac9281-b380-4a83-8acd-eac5ec377342.png">
<br />
2. nella sezione "Api Permission" dell'App registrata di verificare di aver fornito il consenso ammministrativo per l'ambito (function.read);
<br />
<img width="835" alt="auth1" src="https://user-images.githubusercontent.com/3338265/175347696-4af3c1ca-267d-4f18-bf3d-86ac18d65c67.png">
<br />
<img width="835" alt="auth2" src="https://user-images.githubusercontent.com/3338265/175347702-e5c303a6-afe9-4f11-9838-7310ebc3ee28.png">
<br />

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
{<img width="836" alt="ambito" src="https://user-images.githubusercontent.com/3338265/175345703-db242cab-f22b-4533-9172-9571a74fa886.png">

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
