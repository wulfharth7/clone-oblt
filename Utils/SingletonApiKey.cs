using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

//Singleton classes should be sealed all the time to prevent being inherited.
//Source https://refactoring.guru/design-patterns/singleton/csharp/example#example-0


namespace clone_oblt.Utils
{
    public sealed class SingletonApiKey
    {
        //Lazy initialization checks if an instance exists of the class or not. If its not, it get created. Works very well with Singleton Objects.
        private static readonly Lazy<SingletonApiKey> _instance = new Lazy<SingletonApiKey>(() => new SingletonApiKey());
        
        //I'm aware that this is not the best approach to a basic api key, I just wanted to do some extra work here and also not leak the api key accidentally or smth.
        private readonly string _desktopConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ApiKey.json"); // Path to the API key configuration file on the desktop
        
        public string ApiKey { get; private set; }

        private SingletonApiKey()
        {
            LoadApiKey();
        }

        public static SingletonApiKey GetInstance()
        {
            return _instance.Value;
        }

        private void LoadApiKey()
        {
            if (!File.Exists(_desktopConfigPath))
            {
                throw new FileNotFoundException("error, no apikey config.");
            }
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_desktopConfigPath)
            );
            
            if (config.ContainsKey("ObiletApiKey"))
            {
                ApiKey = config["ObiletApiKey"];
            }
            else
            {
                throw new KeyNotFoundException("API key not found in config file.");
            }
        }
    }
}

//this will be thread safe later in order to be scaleable.
