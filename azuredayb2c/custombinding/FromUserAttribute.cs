using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace azuredayb2c.custombinding
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public sealed class FromUserAttribute:Attribute
    {
        public string AutorizedScopes { get; set; }
        public string ClientId { get; set; }
    }
}
