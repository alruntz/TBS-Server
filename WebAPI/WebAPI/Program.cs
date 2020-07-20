using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;


namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static void Shutdown(string message = null)
        {
            if (message != null)
                Console.WriteLine(message);

            Console.WriteLine("Shut down server ...");
            System.Environment.Exit(1);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
