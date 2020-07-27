using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Mine.Commerce.Identity.Areas.Identity.IdentityHostingStartup))]
namespace Mine.Commerce.Identity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                //InitializeDatabase(context.BuildServiceProvider(services), context.Configuration);
            });
        }
        
    }
}