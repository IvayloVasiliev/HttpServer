namespace SIS.HTTP.Sessions
{
    using System.Collections.Generic;

    using SIS.Common;

    public class HttpSession : IHttpSession
    { 
        private readonly Dictionary<string, object> sessionParameters;
                 
        public HttpSession(string id)
        {
            this.Id = id;
            this.sessionParameters = new Dictionary<string, object>();
        }

        public string Id { get; }
        public bool IsNew { get; set; }

        public bool ContainsParameter(string name)
        {
            name.ThrowIfNullOrEmpty(nameof(name));

            return this.sessionParameters.ContainsKey(name);
        }

        public object GetParameter(string name)
        {
            name.ThrowIfNullOrEmpty(nameof(name));

            return this.sessionParameters[name];
        }

        public void AddParameter(string name, object parameter)
        {
            parameter.ThrowIfNull(nameof(parameter));
            name.ThrowIfNullOrEmpty(nameof(name));

            this.sessionParameters[name] = parameter;               
        }

        public void ClearParameters()
        {
            this.sessionParameters.Clear();
        }
    }
}
