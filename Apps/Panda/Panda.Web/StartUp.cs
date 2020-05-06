using Panda.Data;
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
            //serviceProvider.Add
        }
    }
}
