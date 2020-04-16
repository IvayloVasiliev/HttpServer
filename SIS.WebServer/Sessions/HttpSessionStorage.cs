namespace SIS.MvcFramework.Sessions
{
    using System.Collections.Concurrent;

    using HTTP.Sessions;
    using HTTP.Sessions.Contracts;

    public class HttpSessionStorage
    {
        public const string SessionCookieKey = "SIS_ID";

        private static readonly ConcurrentDictionary<string, IHttpSession> httpSessions =
            new ConcurrentDictionary<string, IHttpSession>();

        public static IHttpSession GetSession(string id)
        {
            return httpSessions.GetOrAdd(id, _=> new HttpSession(id));
        }
    }
}
