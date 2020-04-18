namespace SIS.MvcFramework
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework.Attributes;
    using SIS.MvcFramework.Attributes.Action;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Results;
    using SIS.MvcFramework.Routing;
    using System;
    using System.Linq;
    using System.Reflection;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            AutoRegisterRouts(application, serverRoutingTable);

            application.ConfigureServices();

            application.Configure(serverRoutingTable);

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRouts(IMvcApplication application, IServerRoutingTable serverRoutingTable)
        {
           var controllers = application.GetType().Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(Controller).IsAssignableFrom(type));

            foreach (var contoller in controllers)
            {
                var actions = contoller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => !x.IsSpecialName && x.DeclaringType == contoller)
                    .Where(x => x.GetCustomAttributes().All(a => a.GetType() != typeof(NonActionAttribute)));

                foreach (var action in actions)
                {                  
                    var path = $"/{contoller.Name.Replace("Controller", string.Empty)}/{action.Name}";
                    var attribute = action.GetCustomAttributes()
                        .Where(s => s.GetType().IsSubclassOf(typeof(BaseHttpAttribute)))
                        .LastOrDefault() as BaseHttpAttribute;

                    var httpMethod = HttpRequestMethod.Get;

                    if (attribute != null)
                    {
                        httpMethod = attribute.Method; 
                    }

                    if (attribute?.Url != null)
                    {
                        path = attribute.Url;
                    }

                    if (attribute?.ActionName != null)
                    {
                        path = $"/{contoller.Name.Replace("Controller", string.Empty)}/{attribute.ActionName}";
                    }

                    serverRoutingTable.Add(httpMethod, path, request =>
                    {
                        //request => new UsersController().Login(request))
                        var controllerInstance = Activator.CreateInstance(contoller);
                        ((Controller)controllerInstance).Request = request;

                        //Security Authorization
                        var controllerPrincipal = ((Controller)controllerInstance).User;
                        var authorizeAttribute   = action.GetCustomAttributes()
                            .LastOrDefault(a=> a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;

                        if (authorizeAttribute != null && !authorizeAttribute.IsAuthority(controllerPrincipal))
                        {
                            return new HttpResponse(HttpResponseStatusCode.Forbidden);
                        }

                        var responce = action.Invoke(controllerInstance, new object[0]) as ActionResult;
                        return responce;
                    });

                    Console.WriteLine(httpMethod + " " + path);
                }
            }

        }
    }
}
