#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
using Lyrida.UI.Common.Api;
using Lyrida.UI.Common.Filters;
using Lyrida.UI.Common.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Lyrida.UI.Common.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Lyrida.Infrastructure.Common.DependencyInjection;
#endregion

namespace Lyrida.Client;

/// <summary>
/// Application entry point
/// </summary>
/// <remarks>
/// Creation Date: 19th of September, 2023
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
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add(typeof(ApiExceptionFilter));
            options.Filters.Add(typeof(UserTokenActionFilter));
            options.Filters.Add(typeof(EnvironmentFilterAttribute));
        });
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
        });
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        // add filters for cross cutting concerns in regard to the API interaction
        builder.Services.AddScoped<ApiExceptionFilter>();
        builder.Services.AddScoped<UserTokenActionFilter>();
        builder.Services.AddScoped<EnvironmentFilterAttribute>();
        // add session middleware
        builder.Services.AddDistributedMemoryCache();  // required for session state
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        // register the typed client used for the API interaction
        builder.Services.AddHttpClient<IApiHttpClient, ApiHttpClient>();
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            // register global dependencies
            containerBuilder.RegisterModule(new PresentationLayerServices());
            containerBuilder.RegisterModule(new InfrastructureLayerServices());
            // create a child lifetime scope for each user session
            containerBuilder.Register(c => new ChildScopeServiceProviderFactory(c.Resolve<ILifetimeScope>()))
                            .As<IServiceProviderFactory<ContainerBuilder>>();
        });

        var app = builder.Build();
        // ensure the session middleware is applied
        app.UseSession();
        app.UseMiddleware<UserSessionMiddleware>();
        app.UseMiddleware<LanguageMiddleware>();
        app.UseForwardedHeaders();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        string? baseUrl = builder.Configuration["BASE_URL"];
        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            app.UsePathBase(baseUrl);
            Console.WriteLine($"BASE_URL is set to: {baseUrl}");
        }
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.Run();
    }
    #endregion
}