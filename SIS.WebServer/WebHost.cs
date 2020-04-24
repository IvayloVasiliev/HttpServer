﻿namespace SIS.MvcFramework
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework.Attributes;
    using SIS.MvcFramework.Attributes.Action;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.DependencyContainer;
    using SIS.MvcFramework.Logging;
    using SIS.MvcFramework.Results;
    using SIS.MvcFramework.Routing;
    using SIS.MvcFramework.Sessions;
    using System.Linq;
    using System.Reflection;


    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            IHttpSessionStorage httpSessionStorage = new HttpSessionStorage();
            IServiceProvider serviceProvider = new ServiceProvider();
            serviceProvider.Add<ILogger, ConsoleLogger>();

            application.ConfigureServices(serviceProvider);

            AutoRegisterRouts(application, serverRoutingTable, serviceProvider);
            application.Configure(serverRoutingTable);  
            Server server = new Server(8000, serverRoutingTable, httpSessionStorage);
            server.Run();
        }

        private static void AutoRegisterRouts(
            IMvcApplication application, 
            IServerRoutingTable serverRoutingTable,
            IServiceProvider serviceProvider)
        {
           var controllers = application.GetType().Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract 
                && typeof(Controller).IsAssignableFrom(type));

            foreach (var contollerType in controllers)
            {
                var actions = contollerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => !x.IsSpecialName && x.DeclaringType == contollerType)
                    .Where(x => x.GetCustomAttributes().All(a => a.GetType() != typeof(NonActionAttribute)));

                foreach (var action in actions)
                {                  
                    var path = $"/{contollerType.Name.Replace("Controller", string.Empty)}/{action.Name}";
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
                        path = $"/{contollerType.Name.Replace("Controller", string.Empty)}/{attribute.ActionName}";
                    }

                    serverRoutingTable.Add(httpMethod, path, request =>
                    {
                        //request => new UsersController().Login(request))
                        var controllerInstance = serviceProvider.CreateInstance(contollerType) as Controller;
                        controllerInstance.Request = request;

                        //Security Authorization
                        var controllerPrincipal = controllerInstance.User;
                        var authorizeAttribute   = action.GetCustomAttributes()
                            .LastOrDefault(a=> a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;

                        if (authorizeAttribute != null && !authorizeAttribute.IsAuthority(controllerPrincipal))
                        {
                            return new HttpResponse(HttpResponseStatusCode.Forbidden);
                        }

                        var responce = action.Invoke(controllerInstance, new object[0]) as ActionResult;
                        return responce;
                    });

                    System.Console.WriteLine(httpMethod + " " + path);
                }
            }

        }
    }
}
