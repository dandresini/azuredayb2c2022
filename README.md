# Proteggere una Function con Azure Active Directory B2C
Il presente progetto spiega come proteggere le **Azure Function** con **Azure Active Directory B2C**.

Prepariamo il Tenant configurando:
1. Un applicazione di tipo Web che chiameremo **Azure Function** senza alcun URI di reindirizzamento per esporre il nostro ambito di utilizzo (function.read);

<img width="500" alt="app_azure_function" src="https://user-images.githubusercontent.com/3338265/175345163-1b4f0437-b369-40a2-b630-2fbddf9eac54.png">

<img width="500" alt="ambito" src="https://user-images.githubusercontent.com/3338265/175345818-57a6fa4b-d34f-48a6-a109-b5b89cf896f8.png">

2. Un applicazione di tipo Web che chiameremo **Test Function** con reindirizzamento a [jwt.ms](https://jwt.ms) per testare la nostra Azure Function;

<img width="500" alt="Test Function" src="https://user-images.githubusercontent.com/3338265/175346458-200656ca-aba1-422c-9b8a-b60311071e15.png">

3. Un applicazione di tipo SPA (single page application) che chiameremo **React Appt** con reindirizzamento alla nostra app di test in react in locale [http://localhost:3000](http://localhost:3000);

<img width="500" alt="reactapp" src="https://user-images.githubusercontent.com/3338265/175348105-1763a762-51ad-440c-830e-eb463ad704bd.png"/>


Per gli ultimi 2 punti sopra elencati:
1. nella sezione "Authentication" dell'App registrata verificate di avere selezionato quali token rilasciare (nel nostro caso spuntare le voci Access Tokens e ID Tokens);

<img width="500" alt="token" src="https://user-images.githubusercontent.com/3338265/175347564-40ac9281-b380-4a83-8acd-eac5ec377342.png">

2. nella sezione "Api Permission" dell'App registrata verificate di aver fornito il consenso ammministrativo all'ambito (function.read).

<img width="500" alt="auth1" src="https://user-images.githubusercontent.com/3338265/175347696-4af3c1ca-267d-4f18-bf3d-86ac18d65c67.png">

<img width="500" alt="auth2" src="https://user-images.githubusercontent.com/3338265/175347702-e5c303a6-afe9-4f11-9838-7310ebc3ee28.png">


Terminato il Tenant con la configurazione degli User Flows, passate alla creazione della Function: 

<img width="500" alt="function" src="https://user-images.githubusercontent.com/3338265/175353046-aca19bcc-b465-4f20-b46b-a96bc2b56109.png">

<img width="500" alt="authfunction" src="https://user-images.githubusercontent.com/3338265/175354082-3c5a275b-52aa-4e39-999c-d5eac4f7dc8e.png">
Inserire nella sezione

1. Application (client) ID: il Client ID dell App registrata in Azure B2C come **Azure Function**;
2. Client secret (recommended): lasciare vuoto;
3. Issuer URL: inserire l'Endpoint **Azure AD B2C OpenID Connect metadata document** ricordandosi di sostituire **\<policy-name>** con il campo **Name** dello User Flow **"Sign up and sign in"** configurato in precedenza;
4. Allowed token audiences:  il Client ID dell App registrata in Azure B2C come **Azure Function**.


<img width="500" alt="authfunction2" src="https://user-images.githubusercontent.com/3338265/175354086-54c9b9bd-52c9-4cf3-93c6-d48dd380da54.png">


Codice di prova da inserire come primo test
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

Link per scaricare il progetto react dalla documentazione ufficiale Microsoft:
[msal react project](https://github.com/Azure-Samples/ms-identity-javascript-react-spa)

Link problema noto del non funzionamento della "RequiredScope" in una Azure Function:
[RequiredScope not work](https://github.com/AzureAD/microsoft-identity-web/issues/1002)
