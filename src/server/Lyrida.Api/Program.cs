#region ========================================================================= USING =====================================================================================
using Autofac;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Lyrida.Api.Common.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Api.Common.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Lyrida.Domain.Common.DependencyInjection;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.DataAccess.Common.DependencyInjection;
using Lyrida.Application.Common.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Lyrida.Infrastructure.Common.DependencyInjection;
using System.Linq;
using System;
#endregion

namespace Lyrida.Api;

/// <summary>
/// Application entry point, contains the composition root module, wires up all dependencies of the application
/// </summary>
/// <remarks>
/// Creation Date: 04th of July, 2023
/// </remarks>
public class Program
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Application entry point
    /// </summary>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        // add services to the container
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // add authentication and authorization
        builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        // add authentication and specify the JWT scheme to check tokens against
        builder.Services
               .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   //var jwtSettings = app.Services.GetRequiredService<IOptions<JwtSettings>>().Value;
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = "Lyrida",
                       ValidAudience = "Lyrida",
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ds1/L-$#FHj4-}rs^Qq.M/@sP90%j#Ma")), // TODO: remove from code! appConfig.JwtSettings!.SecretKey!
                   };

                   options.Events = new JwtBearerEvents
                   {
                       OnChallenge = context => // event triggered when authentication is not successful for whatever reason
                       {
                           context.HandleResponse(); // prevent the default 401 response
                                                     // set the response status code to 401 Unauthorized
                           context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                           var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                           var problemDetails = problemDetailsFactory.CreateProblemDetails(
                               context.HttpContext,
                               statusCode: StatusCodes.Status401Unauthorized,
                               detail: "You are not authorized"
                           // additional properties as needed
                           );
                           var problemDetailsJson = JsonConvert.SerializeObject(problemDetails);
                           context.Response.ContentType = "application/problem+json";
                           return context.Response.WriteAsync(problemDetailsJson);
                       }
                   };
               });

        builder.Configuration.AddEnvironmentVariables(); 
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            // register dependencies of the application layers
            containerBuilder.RegisterModule(new ApiLayerServices());
            containerBuilder.RegisterModule(new DomainLayerServices());
            containerBuilder.RegisterModule(new DataAccessLayerServices());
            containerBuilder.RegisterModule(new ApplicationLayerServices());
            containerBuilder.RegisterModule(new InfrastructureLayerServices());
        });
        // if environment variables are set, override configuration values with them 
        // (note: asp.net core can do this automatically, but the appsetting.json hierarchy and property names should match - I like more individual environment variables)
        string? environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"] ?? "Production";
        string? databaseHost = builder.Configuration["DATABASE_HOST"];
        string? databasePort = builder.Configuration["DATABASE_PORT"];
        string? databaseName = builder.Configuration["DATABASE_NAME"];
        string? databaseUser = builder.Configuration["DATABASE_USER"];
        string? databasePassword = builder.Configuration["DATABASE_PASSWORD"];
        // only create and set the connection string if all parts are present
        if (new[] { databaseHost, databasePort, databaseName, databaseUser, databasePassword }.All(part => !string.IsNullOrEmpty(part)))
        {
            string connectionString = $"Server={databaseHost};Port={databasePort};Database={databaseName};Uid={databaseUser};Pwd={databasePassword};Convert Zero Datetime=True;Allow User Variables=True;IgnoreCommandTransaction=True";
            // update the IConfiguration directly
            builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
        }
        Console.WriteLine("Environemnt was set to: " + environment);
        var app = builder.Build();
        app.UseExceptionHandler("/error"); // uses a middleware which reexecutes the request to the  error path
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        //app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<EnvironmentMiddleware>();
        app.UseMiddleware<PlatformMiddleware>();
        app.UseMiddleware<LanguageMiddleware>();
        //app.UseMiddleware<AuthorizationMiddleware>();
        app.MapControllers();
        app.Run();
    }
    #endregion
}