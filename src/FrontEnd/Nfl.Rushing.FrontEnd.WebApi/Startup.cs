using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Nfl.Rushing.FrontEnd.Infrastructure.Export;
using Nfl.Rushing.FrontEnd.Infrastructure.Players;

namespace Nfl.Rushing.FrontEnd.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.UseGeneralRoutePrefix("api"); })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddRouting();

            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "Spa"; });
            services.AddCors();

            services.AddSwaggerGen();

            services.AddHttpClient();

            services.AddScoped<IPlayersRepository, PlayersRepository>();
            services.AddScoped<IExportService, CsvExportService>();
            services.AddScoped<IPlayersExportService, PlayersExportService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Configure CORS to allow the spa to communicate with the api while developing locally using react
                app.UseCors(
                    builder => builder.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSpaStaticFiles();
            app.UseSpa(spa => { spa.Options.SourcePath = "Spa"; });
        }
    }
}