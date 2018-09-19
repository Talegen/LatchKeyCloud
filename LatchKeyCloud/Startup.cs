/// <summary>
/// 
/// </summary>
namespace LatchKeyCloud
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.DataProtection;
    using LatchKeyCloud.Data;
    using Microsoft.Extensions.Configuration;
    using LatchKeyCloud.Core.Configuration;
    using Serilog;
    using System.Reflection;
    using System.Threading;
    using System.IO;
    using System.Runtime.InteropServices;
    using LatchKeyCloud.Core;
    using LatchKeyCloud.Core.Caching;
    using Microsoft.AspNetCore.StaticFiles;

    /// <summary>
    /// This class contains the main startup routines for the web application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Contains an <see cref="IConfiguration"/> implementation.</param>
        /// <param name="environment">Contains an <see cref="IHostingEnvironment"/> implementation.</param>
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        /// <summary>
        /// Gets the startup configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Get this the hosting environment.
        /// </summary>
        public IHostingEnvironment Environment { get; }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        public ApplicationSettings Settings { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Contains the service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var settingsSection = this.Configuration.GetSection("ApplicationSettings");
            services.Configure<ApplicationSettings>(settingsSection);
            var settings = settingsSection.Get<ApplicationSettings>();

            string connectionString = this.Configuration.GetConnectionString("DefaultConnection");
            string redisConnectionString = this.Configuration.GetConnectionString("RedisConnection");

            if (settings.ShowDiagnostics)
            {
                Log.Information("Connection String: {0}", connectionString);
                Log.Information("Redis String: {0}", redisConnectionString);
            }

            if (!string.IsNullOrWhiteSpace(redisConnectionString))
            {
                // setup Redis
                RedisManager.Initialize(redisConnectionString);
            }

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // add options support for CORS preflight.
            services.AddOptions();

            // initialize entity framework and init application framework stores
            services
                .AddIdentity<User, IdentityRole<Guid>>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // setup MVC
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddSessionStateTempDataProvider();

            // add session
            services.AddSession();

            // setup distributed caching mechanisms.
            if (!string.IsNullOrWhiteSpace(redisConnectionString))
            {
                services.AddDistributedRedisCache(option =>
                {
                    option.Configuration = redisConnectionString;
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            // if thread settings config has a value...
            if (settings.MinimumCompletionPortThreads > 0)
            {
                // setup threading
                ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
                ThreadPool.SetMinThreads(workerThreads, completionPortThreads > settings.MinimumCompletionPortThreads ? completionPortThreads : settings.MinimumCompletionPortThreads);
            }

            // setup data protection service.
            var dataProtectionService = services.AddDataProtection()
                .SetDefaultKeyLifetime(TimeSpan.FromDays(90))
                .SetApplicationName("LatchKeyCloud");

            // based on the storage method, configure data persistence storage
            switch (settings.PersistenceSettings.StorageMethod)
            {
                case DataPersistenceStorageMethods.Redis:
                    Log.Information("Persisting data to Redis");
                    dataProtectionService.PersistKeysToRedis(RedisManager.Connection, "DataProtection-Keys");
                    break;
                case DataPersistenceStorageMethods.FileSystem:
                    // add data protection with a specific folder to share across a farm.
                    // Do note that to remain secure, the folder must have full permissions for the web application user and no other user.
                    if (!string.IsNullOrWhiteSpace(settings.PersistenceSettings.FolderPath))
                    {
                        Log.Information("Persisting data to folder {0}", settings.PersistenceSettings.FolderPath);

                        // add local file system persistance storage
                        dataProtectionService
                            .PersistKeysToFileSystem(new DirectoryInfo(settings.PersistenceSettings.FolderPath));

                        Log.Information("Environment Version: {0}", RuntimeInformation.OSDescription);

                        // get OS version from description
                        var version = OSPlatformExtensions.GetVersion();

                        // need Windows 8/2012 or newer to support DPAPI-NG
                        if (!string.IsNullOrWhiteSpace(settings.PersistenceSettings.Thumbprint) && (version.Major > 6 || (version.Major == 6 && version.Minor >= 2)))
                        {
                            // use NG encryption if a persistance cert thumbprint was specified and OS supports...

                            if (settings.ShowDiagnostics)
                            {
                                Log.Information("Using modern data protection DPAPI-NG, hash: {0}", settings.PersistenceSettings.Thumbprint);
                            }

                            dataProtectionService.ProtectKeysWithDpapiNG("CERTIFICATE=HashId:" + settings.PersistenceSettings.Thumbprint,
                              flags: Microsoft.AspNetCore.DataProtection.XmlEncryption.DpapiNGProtectionDescriptorFlags.None);
                        }
                        else
                        {
                            // otherwise, use DPAPI method.
                            Log.Information("Using local DPAPI data protection method.");
                            dataProtectionService.ProtectKeysWithDpapi();
                        }
                    }

                    break;
                case DataPersistenceStorageMethods.AzureVault:
                    Log.Information("Persisting data to Azure Vault");
                    dataProtectionService.PersistKeysToAzureBlobStorage(new Uri(settings.PersistenceSettings.AzureBlobUriWithToken));
                    break;
            }

            // if an instrumentation key is specified...
            if (!string.IsNullOrWhiteSpace(settings.InsightsInstrumentationKey))
            {
                services.AddApplicationInsightsTelemetry(settings.InsightsInstrumentationKey);
            }
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Contains the application builder.</param>
        /// <param name="env">Contains the hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var settingsSection = this.Configuration.GetSection("ApplicationSettings");
            var settings = settingsSection.Get<ApplicationSettings>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // if auto-migrate is configured....
            if (settings.AutoMigrate)
            {
                // initialize the database via migrations.
                this.InitializeDatabase(app, env);
            }

            app.UseSession();
            app.UseHttpsRedirection();

            // file extensions provider.
            var extensionsProvider = new FileExtensionContentTypeProvider();

            if (!extensionsProvider.Mappings.ContainsKey(".woff2"))
            {
                extensionsProvider.Mappings.Add(".woff2", "font/woff2");
            }

            app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = extensionsProvider });
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// This method is used to execute database migrations as well as initiate some basic start-up data.
        /// </summary>
        /// <param name="app">Contains the application builder.</param>
        /// <param name="env">Contains the hosting environment.</param>
        private void InitializeDatabase(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    // execute the application database migrations
                    var appContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    appContext.Database.Migrate();

                    // initialize application data.
                    appContext.InitializeApplicationData(env);
                    appContext.InitializeDefaultUserData(serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while attepting to load migration and test data.");
            }
        }
    }
}
