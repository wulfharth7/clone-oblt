﻿using clone_oblt.Builders.Interfaces;
using clone_oblt.Builders;
using clone_oblt.Helpers;
using clone_oblt.Helpers.HelperInterfaces;
using clone_oblt.Models;
using clone_oblt.Services;
using clone_oblt.Services.Interfaces;
using clone_oblt.Utils;
using Newtonsoft.Json.Serialization;

namespace clone_oblt
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //Most of the services are Scoped. For making the code more readable, its seperated as startup/program files.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDistributedMemoryCache();
            services.AddHttpClient<ISessionApiService, SessionApiService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IBusLocationApiService, BusLocationApiService>();
            services.AddScoped<IJourneysApiService, JourneysApiService>();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
            services.AddScoped<ISessionHelperService, SessionHelperService>();
            services.AddScoped<IRequestBuilder<SessionRequest>, SessionRequestBuilder>();
            services.AddScoped<IRequestBuilder<JourneyRequest>, JourneyRequestBuilder>();
            services.AddScoped<IRequestBuilder<BusLocationRequest>, BusLocationBuilder>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy.WithOrigins("http://localhost:3000")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials());
            });


            CreateSession(services);
            
        }
        // Middleware order here is significantly important.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowReactApp");

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Fallback to React app's index.html for other routes
                endpoints.MapFallbackToFile("index.html");
            });
        }

        private void CreateSession(IServiceCollection services)
        {
            //Same-Site option was used when we were testing for front-end development.
            //Because the test environment for our react app was a client, it was sending requests outside of the server.

            //Hence CORS policy were used(removed now because production build code is in the project)
            //And another reason why same-site was set to none. Front-end development isn't finished yet. When its finished, SameSite can be removed.
            services.AddSession(options =>
            {
                options.Cookie.Name = ".Clone.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
        }
    }
}
