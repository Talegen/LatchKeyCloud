namespace LatchKeyCloud
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    /// <summary>
    /// This class is the main entry startup point of the identity server application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Gets or sets an instance of the application configuration.
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        /// <param name="args">Contains an array of command line arguments.</param>
        public static void Main(string[] args)
        {
            // load configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();

            // create configuration
            Configuration = configurationBuilder.Build();

            // setup logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            // startup web server
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        /// <summary>
        /// This method is used to build a new web host instance.
        /// </summary>
        /// <param name="args">Contains an array of command line arguments.</param>
        /// <returns>Returns a new web host instance.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSerilog()
            .UseStartup<Startup>();
    }
}
