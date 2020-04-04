namespace SIS.Demo.Controlers
{
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Home(IHttpRequest request)
        {

            return this.View();
        }
    }
}
