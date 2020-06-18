using EDW_Challenge;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ComponentLoader
{
    public static class Helper
    {
        public static List<Assembly> LoadAllValidAssemblies(string assemblyDirectoryPath, string excludedAssembly)
        {
            List<Assembly> allAssemblies = new List<Assembly>();
           
            var excludeAssemblyRegex = new Regex(excludedAssembly);

            foreach (string dll in Directory.GetFiles(assemblyDirectoryPath, "*.dll"))
            {
                var loadedAssembly = Assembly.LoadFile(dll);
                if (loadedAssembly.CustomAttributes
                    .Any(x => x.AttributeType.Equals(typeof(EDW_ChallengeAttribute)))
                    && !excludeAssemblyRegex.IsMatch(loadedAssembly.FullName)
                    )
                {
                    allAssemblies.Add(loadedAssembly);
                }
            }
            return allAssemblies;
        }

        public static List<IEdwChallenge> GenerateInstanceOfComponent(Assembly assembly,
            string excludedClass, int noofinstance,IConfiguration configuration,ILoggerFactory loggerFactory)
        {
            List<IEdwChallenge> allInstance = new List<IEdwChallenge>();
           
            var excludeClassRegex = new Regex(excludedClass);

            //each class in a assembly filter only class which implements IEdwChallenge interface
            foreach (var type in assembly.GetTypes()
                .Where(x => x.IsClass && typeof(IEdwChallenge).IsAssignableFrom(x)))
            {
                ConstructorInfo[] constructorInfo = type.GetConstructors();
                var ctor = constructorInfo[0];
                bool isConfigExist = false, isLoggerExist = false, isLoggerFactoryExist = false;

                //loop all parameters in constructor
                foreach (var param in ctor.GetParameters())
                {
                    if (param.ParameterType.IsAssignableFrom((typeof(IConfiguration))))
                    {
                        isConfigExist = true;
                    }
                    else if (param.ParameterType.IsAssignableFrom((typeof(ILogger))))
                    {
                        isLoggerExist = true;
                    }
                    else if (param.ParameterType.IsAssignableFrom((typeof(ILoggerFactory))))
                    {
                        isLoggerFactoryExist = true;
                    }

                }

                if (!excludeClassRegex.IsMatch(type.Name) || string.IsNullOrEmpty(excludedClass))
                {
                    //create instance 
                    for (int i = 0; i < noofinstance; i++)
                    {
                        List<object> parameters = new List<object>();

                        if (isConfigExist)
                        {
                            parameters.Add(configuration);
                        }
                        if (isLoggerFactoryExist)
                        {
                            parameters.Add(loggerFactory);

                        }
                        if (isLoggerExist)
                        {
                            parameters.Add(loggerFactory.CreateLogger<object>());
                        }
                        var ai = Activator.CreateInstance(type, parameters.ToArray()) as IEdwChallenge;
                        allInstance.Add(ai);
                    }

                }
            }
            return allInstance;
        }
    }
}
