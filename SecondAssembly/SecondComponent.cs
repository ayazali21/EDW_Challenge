using EDW_Challenge;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SecondAssembly
{
    public class SecondComponent : AbstractComponents ,IEdwChallenge
    {
        private readonly IConfiguration configuration;
        public static Dictionary<Guid, DateTime?> AvailableInstance = new Dictionary<Guid, DateTime?>();

        public SecondComponent(IConfiguration configuration):base()
        {
            this.configuration = configuration;
            AvailableInstance.Add(_guid, null);

        }

        public void Report()
        {
            _lastReported = DateTime.Now;
            AvailableInstance[_guid] = _lastReported;
            Type t = typeof(SecondComponent);
            Assembly assemFromType = t.Assembly;
            PrintConsole(nameof(SecondComponent), assemFromType.FullName,AvailableInstance);

        }
    }
}
