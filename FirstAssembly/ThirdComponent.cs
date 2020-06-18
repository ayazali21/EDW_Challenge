using EDW_Challenge;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FirstAssembly
{
    public class ThirdComponent : AbstractComponents, IEdwChallenge
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        public static Dictionary<Guid, DateTime?> AvailableInstance = new Dictionary<Guid, DateTime?>();


        public ThirdComponent(IConfiguration configuration, ILoggerFactory loggerFactory):base()
        {
            this._configuration = configuration;
            this._loggerFactory = loggerFactory;
            AvailableInstance.Add(_guid, null);

        }

        public void Report()
        {
            _lastReported = DateTime.Now;
            AvailableInstance[_guid] = _lastReported;
            Type t = typeof(ThirdComponent);
            Assembly assemFromType = t.Assembly;
            PrintConsole(nameof(ThirdComponent), assemFromType.FullName, AvailableInstance);

        }
    }
}
