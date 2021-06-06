using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.WebAPI.Filters;
using FahrradladenPrinzenstrasse.WebAPI.Security;
using FahrradladenPrinzenstrasse.WebAPI.Services;
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
using Stripe;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FahrradladenPrinzenstrasse.WebAPI
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
            services.AddCors(options =>
            {
                options.AddPolicy("MyAllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddMvc(x =>
            {
                x.EnableEndpointRouting = false;
                x.Filters.Add<ErrorFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

#pragma warning disable CS0618 // Type or member is obsolete
            services.AddAutoMapper();
#pragma warning restore CS0618 // Type or member is obsolete

            services.AddScoped<IKorisnikService, KorisnikService>();
            services.AddScoped<IBiciklService, BiciklService>();
            services.AddScoped<IOpremaService, OpremaService>();
            services.AddScoped<IServisService, ServisService>();
            services.AddScoped<IKorpaStavkaService, KorpaStavkaService>();
            services.AddScoped<ITerminStavkaService, TerminStavkaService>();
            services.AddScoped<INacinOtpremeService, NacinOtpremeService>();
            services.AddScoped<IRezervacijaService, RezervacijaService>();
            services.AddScoped<IProizvodjacService, ProizvodjacService>();
            services.AddScoped<IVelicinaOkviraService, VelicinaOkviraService>();
            services.AddScoped<IStarosnaGrupaService, StarosnaGrupaService>();
            services.AddScoped<IBojaService, BojaService>();
            services.AddScoped<IBrzinaService, BrzinaService>();
            services.AddScoped<IRecommenderService, RecommenderService> ();
            services.AddScoped<IDioService, DioService> ();
            services.AddScoped<IOcjenaProizvodaService, OcjenaProizvodaService> ();
            services.AddScoped<IGradService, GradService> ();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FahrradladenPrinzenstrasse API v1", Version = "v1" });

                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });

            });

            services.AddAuthentication("BasicAuthentication")
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddDbContext<MyContext>(x =>
                x.UseSqlServer(Configuration.GetConnectionString("plesk")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS0618 // Type or member is obsolete
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            StripeConfiguration.ApiKey = "sk_test_51GtZFeLtVZ5smQUiowBnySeFMXtWtRd8cjejdu71JpHjpwBb2AiE5ZI3CURlv9zK9Qr2w3heajsrPjCJSCbr4sal00RKdS0Mtm";

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseStaticFiles();

            app.UseCors("MyAllowAllOrigins");

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "FahrradladenPrinzenstrasse API v1");
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
