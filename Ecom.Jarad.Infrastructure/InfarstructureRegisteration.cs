using AutoMapper;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Ecom.Jarad.Core.Services;
using Ecom.Jarad.Infrastructure.Data;
using Ecom.Jarad.Infrastructure.Repositries;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Ecom.Jarad.Infrastructure
{
    public static class InfarstructureRegisteration
    {
        public static IServiceCollection infrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            // Injection Connection String
            services.AddDbContext<ApplicationDbContext>(op =>
            {
                op.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                op.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));

            //Add Scoped
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IEmailSender, EmailSendReposirty>();

            // memory cache
            services.AddMemoryCache();

            //config IUnit of work


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //IFile Provider
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            // Identity Role
            services.AddIdentity<AppUsers, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            //Add Authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                // x.RequireHttpsMetadata = false;
                // x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ve...@!.#ryv.][erysecret...@!.#2.][pws@]")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                };
            });

            return services;
        }
    }
}
