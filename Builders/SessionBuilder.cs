using clone_oblt.Models;
using clone_oblt.Builders.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace clone_oblt.Builders
{
    //There are lots of request/response models and other DTOs in this project.
    //At the beginning they were getting created manually and I was just calling the class with a new keyword.

    //For this small amount of models, its fine. But what if the system keeps growing or DTOs have to change, get a new field and other stuff?
    //This is where we use the builder design pattern. It helps to create the DTOs and use them very efficiently, its scalable, readable.
    //And gives us a real easyness, if we want to change them in the future.

    //Hence, I've implemented Builder DP.
    public class SessionRequestBuilder : IRequestBuilder<CreateSessionRequest>
    {
        private int _type;
        private Models.ConnectionInfo _connectionInfo = new Models.ConnectionInfo();
        private BrowserInfo _browserInfo = new BrowserInfo();
        public int Type { get; set; }

        public SessionRequestBuilder WithType(int type)
        {
            _type = type;
            return this;
        }

        public SessionRequestBuilder WithConnection(string ipAddress, string port)
        {
            _connectionInfo.IpAddress = ipAddress;
            _connectionInfo.Port = port;
            return this;
        }

        public SessionRequestBuilder WithBrowser(string name, string version)
        {
            _browserInfo.Name = name;
            _browserInfo.Version = version;
            return this;
        }

        public CreateSessionRequest Build()
        {
            if (_connectionInfo == null || string.IsNullOrEmpty(_connectionInfo.IpAddress) || string.IsNullOrEmpty(_connectionInfo.Port))
            {
                throw new InvalidOperationException("Connection details are required to build the request.");
            }

            if (_browserInfo == null || string.IsNullOrEmpty(_browserInfo.Name) || string.IsNullOrEmpty(_browserInfo.Version))
            {
                throw new InvalidOperationException("Browser details are required to build the request.");
            }

            return new CreateSessionRequest
            {
                Type = _type,
                Browser = _browserInfo,
                Connection = _connectionInfo
            };

        }
    }
}

