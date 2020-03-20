using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.FileReader;
using Blazored.Modal;
using Blazored.Toast;
using jsreport.AspNetCore;
using jsreport.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StocksServer.Data;
using StocksServer.Repositories;
using StocksServer.Repositories.Interfaces;
using StocksServer.Services;

namespace StocksServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddServerSideBlazor().AddHubOptions(o =>
            {
                o.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
            });

            services.AddSingleton<IFactoryRepository, FactoryRepository>();
            services.AddSingleton<IWeeklyProductionRepository, WeeklyProductionRepository>();
            services.AddSingleton<IStocksRepository, StocksRepository>();
            services.AddSingleton<IItemsRepository, ItemsRepository>();
            services.AddJsReport(new ReportingService("http://172.25.194.30:5488"));
            services.AddSingleton<IStocksService, StocksService>();
            services.AddBlazoredToast();
            services.AddBlazoredModal();
            services.AddFileReaderService();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
