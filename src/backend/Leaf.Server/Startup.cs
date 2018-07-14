using System.Collections.Generic;
using System.IO;
using System.Text;
using Leaf.Authorization;
using Leaf.Data;
using Leaf.Data.Configuration;
using Leaf.Environments;
using Leaf.Filters;
using Leaf.Modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Leaf
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        private IConfiguration Configuration { get; }
        private IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors();
            
            // TODO: DataBase 설정 관련 간소화 작업 (DB설정 내부에 쿼리 설정 포함)
            services.AddLeafDataAccess(options =>
            {
                var databases = new List<DatabaseInformationOptions>();
                var queries = new QueryDirectoryOptions();
                
                Configuration.GetSection("databases").Bind(databases);
                Configuration.GetSection("queries").Bind(queries);
                options.Databases = databases;
                options.Queries = queries;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => // TODO: 자체 JwtBearer 설정을 하는 확장메서드 생성
                {
                    var tokenOptions = new TokenAuthenticationOptions();
                    Configuration.GetSection("authentication").Bind(tokenOptions);

                    // TODO: TokenValidationParameters 정보를 중앙 관리하도록 리팩토링
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(tokenOptions.Secret))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnChallenge = (context) =>
                        {
                            // Skip the default logic.
                            context.HandleResponse();
                            // WWW-Authenticate →Bearer error="invalid_token", error_description="The token is expired"
                            context.Response.Headers.Add("WWW-Authenticate",
                                $"Bearer error=\"{context.Error}\", error_description=\"{context.ErrorDescription}\"");
                            context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");

                            var payload = new JObject
                            {
                                ["message"] = context.ErrorDescription,
                                ["error"] = context.Error,
                                ["error_uri"] = context.ErrorUri
                            };
                            return context.Response.WriteAsync(payload.ToString());
                        }
                    };
                });

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IEnvironmentProvider, EnvironmentProvider>();
            services.AddSingleton<JwtTokenProvider>();
            services.AddSingleton<AuthenticationService>();
            services.AddSingleton<AccountRepository>();
            services.AddScoped<ObjectResultExceptionFilterAttribute>();

            services.AddLeafModules(new LeafModulesOptions
            {
                BasePath = Path.Combine(HostingEnvironment.ContentRootPath, "Modules"),
                Shutdown = Program.Shutdown
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());

            app.UseAuthentication();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}