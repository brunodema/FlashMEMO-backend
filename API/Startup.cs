using Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Text;
using Data.Seeder;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Data.Repository.Implementation;
using Business.Services.Implementation;
using Business.Services.Abstract;
using Data.Models.Implementation;
using API.ViewModels;
using Business.Tools.DictionaryAPI.Lexicala;
using Business.Tools.DictionaryAPI.Oxford;
using System.Text.Json.Serialization;

namespace API
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
                options.AddPolicy("AllowConfiguredOrigins", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                );
            }); // try to remove any CORS problems (k8s deployment)

            services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });
            });

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

            services.AddMvc()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                return new BadRequestObjectResult(new BaseResponseModel { Status = "Bad Request", Message = "Validation errors have ocurred when processing the request", Errors = actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            };
        });

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
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                    // custom definitions
                    ValidateLifetime = true, // otherwise the expiration change is not checked
                    ClockSkew = TimeSpan.Zero // the default is 5 min (framework)
                };
            });
            // database configuration
            services.AddDbContext<FlashMEMOContext>(o =>
            {
                o.UseNpgsql(Configuration.GetConnectionString("Postgres"), options => options
                    .MigrationsAssembly("API")
                    .EnableRetryOnFailure(5));
            });
            // options configuration
            services.Configure<JWTServiceOptions>(Configuration.GetSection("JWT"));
            services.Configure<CustomSearchAPIServiceOptions>(Configuration.GetSection("GoogleCustomSearchAPI"));
            services.Configure<OxfordDictionaryAPIRequestHandler>(Configuration.GetSection("OxfordDictionaryAPI"));
            services.Configure<LexicalaDictionaryAPIRequestHandler>(Configuration.GetSection("LexicalaDictionaryAPI"));
            services.Configure<GenericRepositoryServiceOptions>(Configuration.GetSection("BaseRepositoryServiceOptions"));
            // custom services
            services.AddHttpClient();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<NewsService>();
            // Image API
            services.AddScoped<CustomSearchAPIService>();
            // Dictionary API
            services.AddScoped<IDictionaryAPIService<LexicalaAPIResponseModel>, DictionaryAPIService<LexicalaAPIResponseModel>>();
            services.AddScoped<IDictionaryAPIService<OxfordAPIResponseModel>, DictionaryAPIService<OxfordAPIResponseModel>>();
            // Audio API
            services.AddScoped<AudioAPIService>();

            services.AddScoped<ApplicationUserRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<NewsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpLogging(); // use HTTP logging from .NET 6.0 (debug k8s deployment)
             
            app.UseCors("AllowConfiguredOrigins");  // try to remove any CORS problems (k8s deployment)

            //app.UseHttpsRedirection(); // removing for temporary testing

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new BaseResponseModel { Status = "Internal Error", Message = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var seeder = new DbSeeder(provider, Configuration.GetValue<string>("seederPath"));
            seeder.InitializeDatabaseAsync().Wait();
        }
    }
}
