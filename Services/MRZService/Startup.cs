﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTask.EvenBus;
using DevTask.EvenBus.DomainEvents;
using DevTask.RabbitMQ;
using DevTask.MRZService.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevTask.MRZService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<IMRZService>(s => new ABBYYMRZService() { Password = Configuration.GetValue<string>("ABBYYPassword"), ApplicationId = Configuration.GetValue<string>("ABBYYApplicationId") , ServiceUrl = Configuration.GetValue<string>("ABBYServiceURL")});
            services.AddSingleton<IEventBus>(s => new RabbitMQEventBus(Configuration.GetValue<string>("RabbitMQHost"), "kycverfication"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
