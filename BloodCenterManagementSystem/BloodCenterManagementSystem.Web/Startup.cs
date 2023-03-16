using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using BloodCenterManagementSystem.DataAccess;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Web.Queries;
using BloodCenterManagementSystem.Web.Schema;
using BloodCenterManagmentSystem.GraphQL.Types;
using GraphiQl;
using GraphQL;
using GraphQL.Types;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BloodCenterManagementSystem.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BloodBank", Version = "Wersja 1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name="Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type= ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme="oauth2",
                            Name="Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

            });

            services.AddAutoMapper(typeof(Startup));

            services.AddCors(o => o.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            var secretKey = Configuration["SecretKey"];

            var key = Encoding.ASCII.GetBytes(secretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(x =>
            {
                x.AddPolicy("Worker", policyBuilder =>
                 {
                     policyBuilder
                     .RequireAuthenticatedUser()
                     .RequireClaim("Role", "Worker")
                     .Build();
                 });

                x.AddPolicy("Admin", policyBuilder =>
                {
                    policyBuilder
                    .RequireAuthenticatedUser()
                    .RequireClaim("Role", "Admin")
                    .Build();
                });

                x.AddPolicy("Donator", policyBuilder =>
                {
                    policyBuilder
                    .RequireAuthenticatedUser()
                    .RequireClaim("Role", "Donator")
                    .Build();
                });

                x.AddPolicy("WorkerAdmin", policyBuilder =>
                {
                    policyBuilder
                   .RequireAuthenticatedUser()
                   .RequireClaim("Role", new List<string>
                   {
                       "Worker",
                       "Admin"
                   })
                   .Build();
                });

                x.AddPolicy("Authenticated", policyBuilder =>
                {
                    policyBuilder
                    .RequireAuthenticatedUser()
                    .Build();
                });
            });

            services.AddGraphQL(x => SchemaBuilder.New().AddServices(x)
                .AddType<UserType>()
                .AddQueryType<UserQuery>()
                .AddMutationType<UserMutation>()
                .Create());
            /*services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(
                s.GetRequiredService));
            services.AddScoped<ISchema, ProjectSchema>();*/

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BloodCenterManagementSystem");
            });

            app.UsePlayground(new PlaygroundOptions
            {
                QueryPath = "/api",
                Path = "/playground"
            });

            app.UseGraphQL("/api");
        }
    }
}
