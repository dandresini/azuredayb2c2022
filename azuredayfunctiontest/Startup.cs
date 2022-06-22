using azuredayfunctiontest;
using azuredayfunctiontest.custombinding;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace azuredayfunctiontest
{
 
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<FromUserBindingExtensions>();
            builder.Services
                .AddTransient<FromUserAttribute>()
                .AddTransient<FromUserBinding>()
                .AddTransient<FromUserBindingProvider>()
                .AddTransient<FromUserValueProvider>();
        }
    }
}
