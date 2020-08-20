using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.WebAPI.Filters;
using FahrradladenPrinzenstraße.WebAPI.Security;
using FahrradladenPrinzenstraße.WebAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FahrradladenPrinzenstraße.WebAPI
{
    public class BasicAuthDocumentFilter : IDocumentFilter
    {

        public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
        {
            var securityRequirements = new List<OpenApiSecurityRequirement>()
            {
                new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                },
                                Name = "Basic",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    }
            };

            openApiDoc.SecurityRequirements = securityRequirements;
        }
    }


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
            services.AddMvc(x =>
            {
                x.EnableEndpointRouting = false;
                x.Filters.Add<ErrorFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
#pragma warning disable CS0618 // Type or member is obsolete
            services.AddAutoMapper();
#pragma warning restore CS0618 // Type or member is obsolete

            services.AddScoped<IKorisnikService, KorisnikService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FahrradladenPrinzenstraße API v1", Version = "v1" });
                c.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme() { Type = SecuritySchemeType.Http, Scheme = "basic" });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basicAuth" }
                        },
                        new string[]{}
                    }
                });
                //c.DocumentFilter<BasicAuthDocumentFilter>();
            });

            services.AddAuthentication("BasicAuthentication")
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddDbContext<MyContext>(x =>
                x.UseSqlServer(Configuration.GetConnectionString("localDB")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS0618 // Type or member is obsolete
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "FahrradladenPrinzenstraße API v1");
            });

            app.UseMvc();
        }
    }
}
