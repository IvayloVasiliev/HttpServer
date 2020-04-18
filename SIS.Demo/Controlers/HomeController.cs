﻿namespace IRunes.App.Controlers
{
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes;
    using SIS.MvcFramework.Results;

    public class HomeController : Controller
    {
        [HttpGet(Url ="/")]
        public ActionResult IndexSlash()
        {
            return Index();
        }

        public ActionResult Index()
        {
            if (this.IsLoggedIn())
            {
                this.ViewData["Username"] = this.User.Username;

                return this.View("Home");
            }

            return this.View();
        }
    }
}
