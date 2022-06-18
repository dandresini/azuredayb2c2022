using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace azuredayb2c.custombinding
{
    public class FromUserBindingProvider : IBindingProvider
    {
        private readonly ILogger<Startup> logger;
        private readonly FromUserBinding _Binding;


        public FromUserBindingProvider(ILogger<Startup> logger, FromUserBinding binding)
        {
            this.logger = logger;
            _Binding = binding;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            _Binding.NamedArguments = context.Parameter.CustomAttributes.FirstOrDefault()?.NamedArguments;
            return Task.FromResult(_Binding as IBinding);
        }
    }


    public class FromUserBinding : IBinding
    {
        public IList<CustomAttributeNamedArgument> NamedArguments { get; set; }
        
        private readonly ILogger logger;
        private readonly FromUserValueProvider _ValueProvider;
        
        public FromUserBinding(ILogger logger, FromUserValueProvider valueProvider)
        {
            this.logger = logger;
            _ValueProvider = valueProvider;
        }
        
        public Task<IValueProvider> BindAsync(BindingContext context)
        {


            FromUserAttribute userToPass = new FromUserAttribute();
            foreach (var arg in NamedArguments)
            {
                if (_ValueProvider.GetType().GetProperty(arg.MemberName) is not null)
                    _ValueProvider.GetType().GetProperty(arg.MemberName).SetValue(_ValueProvider, arg.TypedValue.Value);
            }

            return Task.FromResult<IValueProvider>(_ValueProvider as IValueProvider);
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => null;
        public bool FromAttribute => true;
        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();

    }
}
