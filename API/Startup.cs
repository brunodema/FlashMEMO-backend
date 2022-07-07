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
using Newtonsoft.Json.Converters;
using FluentValidation.AspNetCore;
using FluentValidation;
using Data.Models.DTOs;
using System.Collections.Generic;
using System.Security.Claims;

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
                           .AllowCredentials()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithOrigins("http://flashmemo.edu:4200", "https://flashmemo.edu:4200")
                );
            }); // try to remove any CORS problems (k8s deployment)

            // implementation to show enums as string taken from here: https://stackoverflow.com/questions/2441290/javascriptserializer-json-serialization-of-enum-as-string. Originally used the system JSON serializer, later changed to Newtonsoft to avoid too much dependency mingling...
            services.AddControllers().AddNewtonsoftJson(o => {
                o.SerializerSettings.Converters.Add(new StringEnumConverter());
                o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field (FOR THE LOVE OF GOD TYPE 'BEARER' AND A WHITESPACE BEFORE THE TOKEN)",
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
                .AddFluentValidation()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        return new BadRequestObjectResult(new BaseResponseModel { Message = "Validation errors have ocurred when processing the request", Errors = actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    };
                });

            // identity config
            services.AddIdentity<User, Role>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<FlashMEMOContext>()
                .AddRoles<Role>()
                .AddDefaultTokenProviders();

            // auth config
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
            });
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
                o.EnableSensitiveDataLogging();
                o.UseNpgsql(Configuration.GetConnectionString("Postgres"), options => options
                    .MigrationsAssembly("API")
                    .EnableRetryOnFailure(5));
            });

            services.AddHttpClient();

            // Options Configuration
            services.Configure<JWTServiceOptions>(Configuration.GetSection("JWT"));
            services.Configure<CustomSearchAPIServiceOptions>(Configuration.GetSection("GoogleCustomSearchAPI"));
            services.Configure<OxfordDictionaryAPIRequestHandler>(Configuration.GetSection("OxfordDictionaryAPI"));
            services.Configure<LexicalaDictionaryAPIRequestHandler>(Configuration.GetSection("LexicalaDictionaryAPI"));
            services.Configure<GenericRepositoryServiceOptions>(Configuration.GetSection("BaseRepositoryServiceOptions"));
            services.Configure<FlashMEMOContextOptions>(Configuration.GetSection("FlashMEMOContextOptions"));
            services.Configure<MailJetOptions>(Configuration.GetSection("MailJet"));
            services.Configure<MailServiceOptions>(Configuration.GetSection("MailService"));
            // Custom Services
            services.AddScoped<NewsService>();
            services.AddScoped<DeckService>();
            services.AddScoped<FlashcardService>();
            services.AddScoped<LanguageService>();
            services.AddScoped<UserService>();
            services.AddScoped<RoleService>();
            // Auth
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IAuthService<string>, AuthService>();
            // Image API
            services.AddScoped<CustomSearchAPIService>();
            // Dictionary API
            services.AddScoped<IDictionaryAPIService<LexicalaAPIResponseModel>, DictionaryAPIService<LexicalaAPIResponseModel>>();
            services.AddScoped<IDictionaryAPIService<OxfordAPIResponseModel>, DictionaryAPIService<OxfordAPIResponseModel>>();
            // Audio API
            services.AddScoped<IAudioAPIService, AudioAPIService>();
            // Email
            services.AddScoped<ISMTPProvider, MailJetSMTPProvider>();
            services.AddScoped<IEmailService, MailJetEmailService>();
            // Repositories (are used in Controllers, for instance)
            services.AddScoped<UserRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<NewsRepository>();
            services.AddScoped<DeckRepository>();
            services.AddScoped<FlashcardRepository>();
            services.AddScoped<LanguageRepository>();

            // Validators
            services.AddTransient<IValidator<FlashcardDTO>, FlashcardDTOValidator>();
            services.AddTransient<IValidator<LanguageDTO>, LanguageDTOValidator>();
            services.AddTransient<IValidator<UserDTO>, UserDTOValidator>();
            services.AddTransient<IValidator<NewsDTO>, NewsDTOValidator>();
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
                var response = new BaseResponseModel { Message = "The back-end server of FlashMEMO ran into a problem.", Errors = new List<string>() { exception.Message } };
                await context.Response.WriteAsJsonAsync(response);
            }));

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // only enable this code if debugging the migration process is necessary
            //using (var scope = app.ApplicationServices.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    var dbContext = services.GetRequiredService<FlashMEMOContext>();

            //    dbContext.Database.Migrate();
            //}
        }
    }
}
