﻿using Panda.Data;
using Panda.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Routing;

namespace Panda.Web
{
    public class StartUp : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            //once on start
            using (var db = new PandaDbContext())
            {
                db.Database.EnsureCreated();
            }

        }

        public void ConfigureServices(SIS.MvcFramework.DependencyContainer.IServiceProvider serviceProvider)
        {
            serviceProvider.Add<IUsersService, UsersService>();
            serviceProvider.Add<IReceiptsService, ReceiptsService>();
            serviceProvider.Add<IPackagesService, PackagesService>();
        }
    }
}
