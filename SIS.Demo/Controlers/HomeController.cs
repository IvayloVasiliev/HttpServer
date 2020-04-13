namespace SIS.Demo.Controlers
{
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses;

    public class HomeController : BaseController
    {
        public HomeController(IHttpRequest httpRequest)
        {
            this.HttpRequest = httpRequest;
        }

        public IHttpResponse Index(IHttpRequest httpRequest)
        {
            //IHttpResponse newResponce = new HttpResponse();
            //HttpCookie cookie = new HttpCookie("lang", "en");
            //newResponce.AddCookie(cookie);
            //return newResponce;

            return this.View();
        }
        
        public IHttpResponse Home(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn())
            {
                return this.Redirect("/login");
            }
            
            this.ViewData["Username"] = this.HttpRequest.Session.GetParameter("username");
            return this.View();
        }
    }
}
