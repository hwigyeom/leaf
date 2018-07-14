using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Leaf
{
    public class Program
    {
        private static readonly CancellationTokenSource CancelTokenSource = new CancellationTokenSource();
        
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            await CreateWebHostBuilder(args).Build().RunAsync(CancelTokenSource.Token);
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
                {
                    var env = webHostBuilderContext.HostingEnvironment;
                    
                    configurationBuilder.AddJsonFile("envsettings.json", optional: true, reloadOnChange: true);
                    configurationBuilder.AddJsonFile($"envsettings.{env.EnvironmentName}.json", optional: true,
                        reloadOnChange: true);
                    configurationBuilder.AddJsonFile("authsettings.json", optional: true, reloadOnChange: true);
                    configurationBuilder.AddJsonFile($"authsettings.{env.EnvironmentName}.json", optional: true, 
                        reloadOnChange: true);
                    configurationBuilder.AddJsonFile("database.json", optional: true, reloadOnChange: true);
                    configurationBuilder.AddJsonFile($"database.{env.EnvironmentName}json", optional: true, 
                        reloadOnChange: true);
                })
                .UseWebRoot("Statics")
                .UseStartup<Startup>();
        }

        public static void Shutdown()
        {
            Console.WriteLine("Application terminate because module directory has changed.");
            CancelTokenSource.Cancel();
        }
    }
}