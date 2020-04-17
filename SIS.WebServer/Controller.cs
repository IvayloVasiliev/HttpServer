namespace SIS.MvcFramework
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Requests;
    using SIS.MvcFramework.Results;


    public abstract class Controller
    {
        protected Controller()
        {
            this.ViewData = new Dictionary<string, object>();
        }

        protected Dictionary<string, object> ViewData;

        private string ParseTemplate(string viewContent)
        {
            foreach (var param in this.ViewData)
            {
                viewContent = viewContent.Replace($"@Model.{param.Key}", param.Value.ToString());
            }
     
            return viewContent;
        }

        protected bool IsLoggedIn(IHttpRequest httpRequest)
        {
            return httpRequest.Session.ContainsParameter("username");
        }

        protected IHttpResponse View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");

            viewContent = this.ParseTemplate(viewContent);

            HtmlResult htmlResult = new HtmlResult(viewContent);

            return htmlResult;
        }

        protected IHttpResponse Redirect(string url)
        {
            return new RedirectResult(url);        
        }

        protected void SignIn(IHttpRequest httpRequest, string id, string userName, string email)
        {
            httpRequest.Session.AddParameter("id", id);
            httpRequest.Session.AddParameter("username", userName);
            httpRequest.Session.AddParameter("email", email);
            
        }

        protected void SignOut(IHttpRequest httpRequest)
        {
            httpRequest.Session.ClearParameters();
        }
    }
}
