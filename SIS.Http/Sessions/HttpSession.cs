﻿namespace SIS.HTTP.Sessions
{
    using System.Collections.Generic;

    using Common;
    using Sessions.Contracts;

    public class HttpSession : IHttpSession
    { 
        private readonly Dictionary<string, object> sessionParameters;
                 
        public HttpSession(string id)
        {
            this.Id = id;
            this.sessionParameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public bool ContainsParameter(string name)
        {
            CoreValidator.ThrowIfNullOrEmpty(name, nameof(name));

            return this.sessionParameters.ContainsKey(name);
        }

        public object GetParameter(string name)
        {
            CoreValidator.ThrowIfNullOrEmpty(name, nameof(name));

            return this.sessionParameters[name];
        }

        public void AddParameter(string name, object parameter)
        {
            CoreValidator.ThrowIfNullOrEmpty(name, nameof(name));
            CoreValidator.ThrowIfNull(parameter, nameof(parameter));

            this.sessionParameters[name] = parameter;               
        }

        public void ClearParameters()
        {
            this.sessionParameters.Clear();
        }
    }
}
