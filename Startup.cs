using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using pizzastore.Models;
using pizzastore.Services;

namespace pizzastore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly TokenService _token = new TokenService();

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        string[] allowedOrigins = {"http://localhost:3000"};
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors( options => {
                options.AddPolicy(name: MyAllowSpecificOrigins, builder => {
                                    builder.WithOrigins(allowedOrigins[0]).AllowAnyMethod();
                });
            });
            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));
            services.AddSingleton<IDatabaseSettings>(x => x.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            services.AddSingleton<UserService>();
            services.AddSingleton<PizzaService>();
            services.AddSingleton<TokenService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("JetKey").ToString())),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

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

            app.Use(async (context, next) => {
                context.Response.Headers["Access-Control-Allow-Origin"] = allowedOrigins[0];
                context.Response.Headers["Access-Control-Allow-Headers"] = "*";
                
                await next.Invoke();
            });

            // app.Use(async (context, next) => {

            //     var request = context.Request;
            //     var headers = request.Headers;
            //     var handler = new JwtSecurityTokenHandler();
            //     var token = headers["Authorization"].ToString().Replace("Bearer ", "");
            //     _token.TokenStatus(token);
            //     if(!_token.TokenStatus(token)) {
            //         var response = context.Response;
            //         StreamReader reader = new StreamReader("bad Token");
            //         string text = reader.ReadToEnd();
            //         context.Response.StatusCode = 401;
            //         context.Response.ContentType = "application/json";
            //         context.Response.Body = reader.BaseStream;

            //         return;
            //     }
            //     await next.Invoke();
            // });
            app.UseAuthentication();
            app.UseCors(MyAllowSpecificOrigins);

            // app.UseHttpsRedirection(); commenting for development environment
            app.UseMvc();
        }
    }
}
