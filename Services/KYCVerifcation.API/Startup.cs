﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTask.EvenBus;
using DevTask.EvenBus.DomainEvents;
using DevTask.KYCVerification.Domain.Dbcontexts;
using DevTask.RabbitMQ;
using KYCVerifcation.API.EventHandlers;
using KYCVerifcation.API.Servces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KYCVerifcation.API
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
            services.AddTransient<IKYCVerifcationService>(s=>  new TruliooKYCVerifcationService() { Key = Configuration.GetValue<string>("TruliooKey") ,TruilooUrl = Configuration.GetValue<string>("TruilooUrl") });
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

            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();           
            eventBus.Subscribe("MRZVerifiedEvent", typeof(MRZVerifiedEventHandler));
        }
    }
}
