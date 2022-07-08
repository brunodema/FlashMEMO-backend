﻿using System;
using System.Linq;
using System.Text;
using API.Controllers;
using API.ViewModels;
using Business.Services.Implementation;
using Business.Services.Interfaces;
using Data.Context;
using Data.Models.DTOs;
using Data.Models.Implementation;
using Data.Repository.Implementation;
using Data.Seeder;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Tests.Integration.Fixtures
{
    public class TestStartup
    {
        private class InternalConfigs
        {
            // JWT
            public const string ValidIssuer = "http://localhost:61955";
            public const string ValidAudience = "http://localhost:4200";
            public const int AccessTokenTTE = 300;
            public const int RefreshTokenTTE = 7776000; // 90 days
            public const string Secret = "mysecretmysecret";
            // Seeder
            public const string SeederPath = "../../../../Data/Seeder";
            public const string DefaultPassword = "DefaultPassword@123";
        }

        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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

            services.AddMvc()
                .AddFluentValidation()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        return new BadRequestObjectResult(new BaseResponseModel { Message = "Validation errors have ocurred when processing the request", Errors = actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    };
                })
                .AddApplicationPart(typeof(NewsController).Assembly) // why am I doing this again?
                .AddApplicationPart(typeof(AuthController).Assembly)
                .AddApplicationPart(typeof(OxfordDictionaryAPIController).Assembly)
                .AddApplicationPart(typeof(LexicalaDictionaryAPIController).Assembly)
                .AddApplicationPart(typeof(ImageAPIController).Assembly);

            // identity config
            services.AddIdentity<User, Role>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<FlashMEMOContext>()
                .AddRoles<Role>()
                .AddDefaultTokenProviders();
            // Configure password reset tokens
            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromSeconds(Double.Parse(Configuration["IdentitySecurity:PasswordTokenTTE"])));

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
                        ValidAudience = InternalConfigs.ValidAudience,
                        ValidIssuer = InternalConfigs.ValidIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(InternalConfigs.Secret)),
                        // custom definitions
                        ValidateLifetime = true, // otherwise the expiration change is not checked
                        ClockSkew = TimeSpan.Zero // the default is 5 min (framework)
                    };
                });

            // database configuration
            services.AddDbContext<FlashMEMOContext>(options => options.UseInMemoryDatabase("TestDB"));
            services.AddSingleton(opt => Options.Create<FlashMEMOContextOptions>(new FlashMEMOContextOptions()
            {
                SeederPath = InternalConfigs.SeederPath,
                DefaultUserPassword = InternalConfigs.DefaultPassword
            }));

            // options configuration
            services.Configure<JWTServiceOptions>(config =>
            {
                config.ValidIssuer = InternalConfigs.ValidIssuer;
                config.ValidAudience = InternalConfigs.ValidAudience;
                config.AccessTokenTTE = InternalConfigs.AccessTokenTTE;
                config.RefreshTokenTTE = InternalConfigs.RefreshTokenTTE;
                config.Secret = InternalConfigs.Secret;
            });

            services.Configure<CustomSearchAPIServiceOptions>(Configuration.GetSection("GoogleCustomSearchAPI"));
            services.Configure<OxfordDictionaryAPIRequestHandler>(Configuration.GetSection("OxfordDictionaryAPI"));
            services.Configure<LexicalaDictionaryAPIRequestHandler>(Configuration.GetSection("LexicalaDictionaryAPI"));
            services.Configure<FlashMEMOContextOptions>(Configuration.GetSection("FlashMEMOContextOptions"));
            services.Configure<MailServiceOptions>(Configuration.GetSection("MailService"));


            // custom services
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IAuthService<string>, AuthService>();
            services.AddScoped<NewsService>();
            services.AddScoped<CustomSearchAPIService>();
            services.AddScoped<DeckService>();
            services.AddScoped<LanguageService>();
            services.AddScoped<FlashcardService>();
            services.AddScoped<UserService>();
            // Email
            services.AddScoped<ISMTPProvider, MockSMTPProvider>(); // I'm using this to not burn up the API quotas
            services.AddScoped<IEmailService, MailJetEmailService>();

            services.AddScoped<UserRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<NewsRepository>();
            services.AddScoped<DeckRepository>();
            services.AddScoped<LanguageRepository>();
            services.AddScoped<FlashcardRepository>();

            // Validators
            services.AddTransient<IValidator<FlashcardDTO>, FlashcardDTOValidator>();
            services.AddTransient<IValidator<NewsDTO>, NewsDTOValidator>();
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

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new BaseResponseModel { Message = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));

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
        }
    }
}