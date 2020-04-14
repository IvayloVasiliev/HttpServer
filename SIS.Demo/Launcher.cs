namespace IRunes.App
{
    using IRunes.App.Controlers;
    using IRunes.Data;
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;

    public class Launcher
    {
        public static void Main(string[] args)
        {
            using (var context = new RunesDbContext())
            {
                context.Database.EnsureCreated();
            }

            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            Configure(serverRoutingTable);

            Server server = new Server(8000, serverRoutingTable);
            server.Run(); 
        }

        private static void Configure(ServerRoutingTable serverRoutingTable)
        {
            #region Home Routs
            serverRoutingTable.Add(HttpRequestMethod.Get, "/", request => new RedirectResult("/Home/Index"));
            serverRoutingTable.Add(HttpRequestMethod.Get, "/Home/Index", request => new HomeController().Index(request));
            #endregion

            #region USers Routs
            serverRoutingTable.Add(HttpRequestMethod.Get, "/Users/Login", request => new UsersController().Login(request));
            serverRoutingTable.Add(HttpRequestMethod.Post, "/Users/Login", request => new UsersController().LoginConfirm(request));
            serverRoutingTable.Add(HttpRequestMethod.Get, "/Users/Register", request => new UsersController().Register(request));
            serverRoutingTable.Add(HttpRequestMethod.Post, "/Users/Register", request => new UsersController().RegisterConfirm(request));
            serverRoutingTable.Add(HttpRequestMethod.Get, "/Users/Logout", request => new UsersController().Logout(request));

            #endregion

        }
    }
}
