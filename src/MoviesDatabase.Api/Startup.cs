﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoviesDatabase.Api.Configuration;

namespace MoviesDatabase.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.RegisterDependencies(Configuration);

            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc().UseSwaggerUI(Configuration);
        }
    }

    public class StartupDevelopment : Startup
    {
        public StartupDevelopment(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(options =>
            {
                options
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            })
                .UseDeveloperExceptionPage()
                .SeedFakeData();

            base.Configure(app, env);
        }
    }

    public class StartupProduction : Startup
    {
        public StartupProduction(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseApiExceptionHandler();

            base.Configure(app, env);
        }
    }
}
