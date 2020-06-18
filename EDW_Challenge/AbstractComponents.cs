using EDW_Challenge;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EDW_Challenge
{
    public abstract class AbstractComponents
    {
        protected readonly Guid _guid;
        protected DateTime? _lastReported = null;
        public AbstractComponents()
        {
            _guid = Guid.NewGuid();

        }

        public void PrintConsole(string className, string assemblyName, Dictionary<Guid, DateTime?> availableInstance)
        {
            var otherRunningInstance = availableInstance.Where(x => x.Key != _guid)
                .Select(x => $"\t\t Guid ={x.Key}, ClassName ={className} " +
             $"and Report Method called on {x.Value} ").ToArray();
            Console.WriteLine($"Guid :{_guid}\n" +
                            $"Class Name : {className}\n" +
                            $"Assembly Name:{assemblyName}\n" +
                            $"Other Running Instance :\n {string.Join("\n", otherRunningInstance)}");
            Console.WriteLine("\n---------------------------\n");
        }
    }

}
