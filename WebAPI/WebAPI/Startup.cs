using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using WebAPI.Security;
using TBS.Models;

namespace WebAPI
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
            services.AddScoped<WebAPI.Services.PlayerService>();
            services.AddScoped<WebAPI.Services.UserService>();
            services.AddScoped<WebAPI.Services.AuthenticationService>();
            services.AddScoped<WebAPI.Services.CharacterService>();
            services.AddScoped<WebAPI.Services.ItemService>();
            services.AddScoped<WebAPI.Services.SpellService>();
            services.AddScoped<WebAPI.Services.MapService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Security.Authentication.Issuer,
                    ValidAudience = Security.Authentication.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Security.Authentication.SecretKey))
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Private", policy =>
                    policy.Requirements.Add(new RoleAuthorizationRequirement(UserRole.User)));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.Requirements.Add(new RoleAuthorizationRequirement(UserRole.Administrator)));
            });

            services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();

            TBSEngine.Database.LoadAll(Services.SpellService.Instance.GetAll().Result,
                Services.CharacterService.Instance.GetAll().Result,
                Services.PlayerService.Instance.GetPlayers().Result,
                Services.ItemService.Instance.GetAll().Result);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
