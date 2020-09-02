using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDbTest.Configuration;

namespace MongoDbTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IWebHost webHost = CreateWebHostBuilder(args).Build();

            // Create a new scope
            using (var scope = webHost.Services.CreateScope())
            {
                // Get the DbContext instance
                var myDbContext = scope.ServiceProvider.GetRequiredService<IDataBaseInitializer>();

                //Do the migration asynchronously
                await myDbContext.InitDatabase();
            }

            // Run the WebHost, and start accepting requests
            // There's an async overload, so we may as well use it
            await webHost.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseStartup<Startup>();
    }
}
