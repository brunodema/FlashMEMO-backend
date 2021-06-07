using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using API.ViewModels;
using Business.Services;
using Business.Services.Interfaces;
using Data.Context;
using Data.Models;
using Data.Repository;
using Data.Repository.Interfaces;
using Data.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Tests.Integration.Fixtures
{
    public class TestStartup
    {
        private class InteralConfigs
        {
            // JWT
            public const string ValidIssuer = "http://localhost:61955";
            public const string ValidAudience = "http://localhost:4200";
            public const int TimeToExpiration = 300;
            public const string Secret = "mysecretmysecret";
            // Seeder
            public const string SeederPath = "../../..//../Data/Seeder";
        }

        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        In = ParameterLocation.Header,
            //        Description = "Please insert JWT with Bearer into field",
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey
            //    });
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            //       {
            //         new OpenApiSecurityScheme
            //         {
            //           Reference = new OpenApiReference
            //           {
            //             Type = ReferenceType.SecurityScheme,
            //             Id = "Bearer"
            //           }
            //          },
            //          new string[] { }
            //        }
            //      });
            //});

            // custom definitions
            // swagger/API definitions
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

            services.AddMvc().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new BadRequestObjectResult(new BaseResponseModel { Status = "Bad Request", Message = "Validation errors have ocurred when processing the request", Errors = actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                };
            })
            .AddApplicationPart(typeof(NewsController).Assembly)
            .AddApplicationPart(typeof(AuthController).Assembly);

            // identity config
            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<FlashMEMOContext>()
                .AddRoles<ApplicationRole>()
                .AddDefaultTokenProviders();
            // auth config
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = InteralConfigs.ValidAudience,
                        ValidIssuer = InteralConfigs.ValidIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(InteralConfigs.Secret)),
                        // custom definitions
                        ValidateLifetime = true, // otherwise the expiration change is not checked
                        ClockSkew = TimeSpan.Zero // the default is 5 min (framework)
                    };
                });
            // database configuration
            services.AddDbContext<FlashMEMOContext>(options => options.UseInMemoryDatabase("TestDB"));
            // options configuration
            services.Configure<JWTServiceOptions>(config =>
            {
                config.ValidIssuer = InteralConfigs.ValidIssuer;
                config.ValidAudience = InteralConfigs.ValidAudience;
                config.TimeToExpiration = InteralConfigs.TimeToExpiration;
                config.Secret = InteralConfigs.Secret;
            });
            // custom services
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<NewsService>();
            services.AddScoped<ApplicationUserRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<NewsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            //custom configuration - CORS for development
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            DbSeeder.InitializeDatabaseAsync(provider, "../../..//../Data/Seeder").Wait();
        }
    }
}