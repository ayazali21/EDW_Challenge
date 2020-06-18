using EDW_Challenge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

[assembly: EDW_ChallengeAttribute]

namespace FirstAssembly
{
    public class FirstComponent : AbstractComponents, IEdwChallenge
    {
        public static Dictionary<Guid, DateTime?> AvailableInstance = new Dictionary<Guid, DateTime?>();

        public FirstComponent() : base()
        {
            AvailableInstance.Add(_guid, null);

        }
        public void Report()
        {
            _lastReported = DateTime.Now;
            AvailableInstance[_guid] = _lastReported;
            Type t = typeof(FirstComponent);
            Assembly assemFromType = t.Assembly;
            PrintConsole(nameof(FirstComponent), assemFromType.FullName, AvailableInstance);

        }

    }
}
