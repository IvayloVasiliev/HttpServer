using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.Controlers
{
    public class InfoController : Controller
    {
        public IHttpResponse About(IHttpRequest httpRequest)
        {
            return this.View();
        }

    }
}
