using ContaFinanceira.Email.Application.Services;
using ContaFinanceira.Email.Application.Validations;
using ContaFinanceira.Email.Domain.Interfaces;
using ContaFinanceira.Email.Domain.Requests;
using ContaFinanceira.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContaFinanceira.Email.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .AddFluentValidation();

            services.AddTransient<IValidator<TransacaoRequest>, TransacaoRequestValidation>();

            services.AddTransient<IEmailService, EmailService>();

            services.AddCors(x =>
            {
                x.AddPolicy("Conta Financeira Email Policy",
                            builder =>
                            {
                                builder.AllowAnyMethod()
                                       .AllowAnyHeader()
                                       .AllowAnyOrigin();
                            });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Conta Financeira Email", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conta Financeira Email"));
            }

            app.UseMiddleware<LoggerMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(x => x.AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowAnyOrigin())
               .UseCors("Conta Financeira Email Policy");
        }
    }
}