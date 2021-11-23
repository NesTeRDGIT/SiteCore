using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SiteCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ServiceLoaderMedpomData;
using SiteCore.Class;
using SiteCore.Controllers;
using SiteCore.Hubs;
using Path = System.IO.Path;


namespace SiteCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseOracle(Configuration.GetConnectionString("DefaultConnection"), b=>b.UseOracleSQLCompatibility("11")));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddUserManager<ApplicationUserManager>().AddDefaultTokenProviders();

            services.AddTransient<AccountHelper>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsIdentityFactory>();
            services.AddControllersWithViews().AddSessionStateTempDataProvider();
            services.AddRazorPages();
            services.Replace(new ServiceDescriptor(serviceType: typeof(IPasswordHasher<ApplicationUser>), implementationType: typeof(Mvc5MvcPasswordHasher), ServiceLifetime.Scoped));
            services.AddSignalR();
            services.AddSession();


            services.AddBase(Configuration);
            services.AddMedpom(Configuration);
            services.AddIdentiCS(Configuration);
            services.AddTMK(Configuration);
            services.AddMSE(Configuration);
            services.AddONK(Configuration);
            services.AddZPZ(Configuration);
            services.AddSTAC(Configuration);
            services.AddSIGN(Configuration);
            services.AddReport(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }


            // Set up custom content types - associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider
            {
                Mappings =
                {
                    // Add new mappings
                    [".xpi"] = "application/x-xpinstall",
                    [".crx"] = "application/x-chrome-extension"
                }
            };
          
            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = provider
            });

          
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<NotificationHub>("/Notification");
            });


        }
    }


    public static class ExtIServiceCollection
    {


        public static IServiceCollection AddBase(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient(x => new UserInfoHelper(x.GetService<ApplicationUserManager>()));
            services.AddTransient<IHasher>(provider => new GostHasher());
            services.AddTransient<IZipArchiver>(provider => new ZipArchiver());
            services.AddTransient<ILogger>(provider => new LoggerEventLog("SiteCore"));
            return services;
        }



        public static IServiceCollection AddMedpom(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<MyOracleSet>(options => options.UseOracle(Configuration.GetConnectionString("DefaultConnection"), b => b.UseOracleSQLCompatibility("11")));
            var host = Configuration.GetSection("WCFParam").GetValue("HOST", "");
            var login = Configuration.GetSection("WCFParam").GetValue("LOGIN", "");
            var password = Configuration.GetSection("WCFParam").GetValue("PASSWORD", "");
            services.AddSingleton(provider => new WCFConnect(host, login, password, provider.GetService<ILogger>(), provider.GetService<IHubContext<NotificationHub>>()));
            var SharedFolder = Configuration.GetValue<string>("SharedFolder");
           
            services.AddTransient<IMedpomRepository>(provider => new MedpomFileManager(SharedFolder));          
            return services;
        }
        public static IServiceCollection AddIdentiCS(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<CSOracleSet>(options => options.UseLazyLoadingProxies().UseOracle(Configuration.GetConnectionString("DefaultConnection"), b => b.UseOracleSQLCompatibility("11")));
            var host = Configuration.GetSection("WCFIdentiCSParam").GetValue("HOST", "");          
            services.AddSingleton(provider => new WCFIdentiScaner(host, provider.GetService<ILogger>(), provider.GetService<IHubContext<NotificationHub>>(), services.BuildServiceProvider().GetService<CSOracleSet>()));
            return services;
        }

        public static IServiceCollection AddTMK(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<TMKOracleSet>(options => options.UseLazyLoadingProxies().UseOracle(Configuration.GetConnectionString("TMKConnection"), b => b.UseOracleSQLCompatibility("11")));
            services.AddTransient<ITMKExcelCreator>(provider =>
            {
                var pathTemplate = Path.Combine(provider.GetService<IWebHostEnvironment>().WebRootPath, "Template", "TMK");
                return new TMKExcelCreator(Path.Combine(pathTemplate, "Reestr.xlsx"), Path.Combine(pathTemplate, "Report.xlsx"));

            });
            return services;
        }

        public static IServiceCollection AddMSE(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<MSEOracleSet>(options => options.UseLazyLoadingProxies().UseOracle(Configuration.GetConnectionString("MSEConnection"), b => b.UseOracleSQLCompatibility("11")));
            services.AddTransient<IMSEExcelCreator>(provider =>
            {
                var pathTemplate = Path.Combine(provider.GetService<IWebHostEnvironment>().WebRootPath, "Template", "MSE");
                return new MSEExcelCreator(Path.Combine(pathTemplate, "Report.xlsx"), Path.Combine(pathTemplate, "MSE.xlsx"));

            });

            return services;
        }
        public static IServiceCollection AddONK(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ONKOracleSet>(options => options.UseOracle(Configuration.GetConnectionString("ONKConnection"), b => b.UseOracleSQLCompatibility("11")));
            return services;
        }
        public static IServiceCollection AddZPZ(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<IZPZExcelCreator>(provider =>
            {
                var pathTemplate = Path.Combine(provider.GetService<IWebHostEnvironment>().WebRootPath, "Template", "ZPZ");
                return new ZPZExcelCreator(Path.Combine(pathTemplate, "Template1.xlsx"), Path.Combine(pathTemplate, "ResultControlTemplate.xlsx"));
            });
            return services;
        }
        public static IServiceCollection AddSTAC(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<ISTAC_PLANExcelCreator>(provider =>
            {
                var pathTemplate = Path.Combine(provider.GetService<IWebHostEnvironment>().WebRootPath, "Template", "STAC");
                return new STAC_PLANExcelCreator(Path.Combine(pathTemplate, "TAB1.xlsx"), Path.Combine(pathTemplate, "TAB3.xlsx"), Path.Combine(pathTemplate, "TAB4.xlsx"));
            });
            return services;
        }
        public static IServiceCollection AddSIGN(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<IX509CertificateManager>(provider => new X509CertificateManager(provider.GetService<MyOracleSet>()));
            var host = Configuration.GetSection("WCFCrypto").GetValue("HOST", "");
            services.AddSingleton(provider => new WCFCryptoConnect(host, provider.GetService<ILogger>()));            
            return services;
        }

        public static IServiceCollection AddReport(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<IOMSXLSCreator>(provider =>
            {
                var pathTemplate = Path.Combine(provider.GetService<IWebHostEnvironment>().WebRootPath, "Template", "OOMS");
                return new OMSXLSCreator(Path.Combine(pathTemplate, "DISP_TEMPLATE.xlsx"), Path.Combine(pathTemplate, "KV2_MTR_TEMPLATE.xlsx"), Path.Combine(pathTemplate, "DLI_TEMPLATE.xlsx"));

            });
            return services;
        }


    }


}
