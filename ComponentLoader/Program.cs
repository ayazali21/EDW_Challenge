using EDW_Challenge;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComponentLoader
{
    class Program
    {
        private static IConfiguration _config;
        private static ILoggerFactory _loggerFactory;

        static  void Main(string[] args)
        {
            List<IEdwChallenge> availableInstances = new List<IEdwChallenge>();
            int noofinstance = 1;
            string excludedAssembly = string.Empty, excludedClass = string.Empty;


            //extract the command line arguments 
            if (args.Any(x => x.StartsWith("-excludeAssembly=")))
            {
                excludedAssembly = args.First(x => x.StartsWith("-excludeAssembly="))
                    .Split("-excludeAssembly=").Last();

            }

            //excluded class per assembly
            if (args.Any(x => x.StartsWith("-excludeClass=")))
            {
                excludedClass = args.First(x => x.StartsWith("-excludeClass=")).Split("-excludeClass=").Last();

            }

            //number of instance per class
            if (args.Any(x => x.StartsWith("-instances=")))
            {
                noofinstance = Convert.ToInt16(args.First(x => x.StartsWith("-instances="))
                    .Split("-instances=").Last());
            }

            InitializeBuilder();

            string assemblyDirectory = _config["AssemblyDirectoryPath"];

            //load valid avaialble assembly
            var validAssemblies = Helper.LoadAllValidAssemblies(assemblyDirectory, excludedAssembly);

            //create instance of class in each assembly
            
           

            foreach (var assembly in validAssemblies)
            {
                availableInstances.AddRange(Helper.GenerateInstanceOfComponent(assembly,excludedClass,
                    noofinstance,_config,_loggerFactory));
            }

            //call the Report Method from all instances

            var childTask = Task.Run(() =>
            {
                Parallel.ForEach(availableInstances, (instance) =>
                {
                    instance.Report();
                });
            });
            childTask.Wait();
        }

        public static void InitializeBuilder()
        {

            _config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", true, true)
               .Build();
            _loggerFactory = new LoggerFactory();
        }
        
       
    }
}
