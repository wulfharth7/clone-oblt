using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace clone_oblt.Utils
{
    //Singleton classes should be sealed all the time to prevent being inherited. Source https://refactoring.guru/design-patterns/singleton/csharp/example#example-0
    public sealed class SingletonApiKey
    {
        //Lazy checks if an instance exists of the class or not. If its not, it get created. Works very well with Singleton Objects tbh.
        private static readonly Lazy<SingletonApiKey> _instance = new Lazy<SingletonApiKey>(() => new SingletonApiKey());
        public string ApiKey { get; private set; }
        // Path to the API key configuration file on the desktop
        private readonly string _desktopConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ApiKey.json");

        //Constructor
        private SingletonApiKey()
        {
            LoadApiKey();
        }

        //Function for getting the singleton object
        public static SingletonApiKey GetInstance()
        {
            return _instance.Value;
        }

        //Method to load the API key from the file and set the ApiKey property
        private void LoadApiKey()
        {
            if (!File.Exists(_desktopConfigPath))
            {
                throw new FileNotFoundException("API configuration file not found on the desktop.");
            }
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_desktopConfigPath)
            );
            
            //checks if the json key exists or not.
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
