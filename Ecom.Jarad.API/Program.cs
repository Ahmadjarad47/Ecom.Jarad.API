using Ecom.Jarad.API.Errors;
using Ecom.Jarad.API.Midllware;
using Ecom.Jarad.Core.Interfaces;
using Ecom.Jarad.Infrastructure;
using Ecom.Jarad.Infrastructure.Repositries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Ecom.Jarad.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //use infrastructure Service
            builder.Services.infrastructure(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(op =>
            {
                var securty = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "jwt Auth Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                op.AddSecurityDefinition("Bearer", securty);

                OpenApiSecurityRequirement SR = new OpenApiSecurityRequirement { { securty, new[] { "Bearer" } } };

                op.AddSecurityRequirement(SR);
            });



            builder.Services.AddControllers().ConfigureApiBehaviorOptions(op =>
             {
                 op.InvalidModelStateResponseFactory = context =>
                 {
                     APIValidationError error = new APIValidationError
                     {
                         Error = context.ModelState.Where(x => x.Value.Errors.Count() > 0)
                         .SelectMany(x => x.Value.Errors)
                         .Select(x => x.ErrorMessage),
                     };
                     return new BadRequestObjectResult(error);
                 };
             });



            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(m => m.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddliWare>();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}