using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Logging;

namespace azuredayfunctiontest.custombinding
{
    [Extension("FromUserBindingExtensions")]
    public class FromUserBindingExtensions : IExtensionConfigProvider
    {
        private readonly ILogger logger;
        private readonly IBindingProvider _BindingProvider;
        public FromUserBindingExtensions(ILogger<Startup> logger, FromUserBindingProvider bindingProvider)
        {
            this.logger = logger;
            _BindingProvider = bindingProvider;
        }
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<FromUserAttribute>()
                .Bind(_BindingProvider);
        }
    }
}
