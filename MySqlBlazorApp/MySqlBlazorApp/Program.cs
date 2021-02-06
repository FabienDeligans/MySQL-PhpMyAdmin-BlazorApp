using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace MySqlBlazorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($@"###################################################");
            Console.WriteLine($@"# Application MySqlBlazorApp en cours d'exécution #");
            Console.WriteLine($@"###################################################");

            Console.WriteLine($@"");
            Console.WriteLine($@"");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
