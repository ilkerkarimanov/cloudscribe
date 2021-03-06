﻿
using cloudscribe.Core.Identity;
using cloudscribe.Core.IdentityServerIntegration;
using cloudscribe.Core.Models;
using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Hosting;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

// https://github.com/IdentityServer/IdentityServer4/issues/19

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IIdentityServerBuilder AddCloudscribeIdentityServerIntegration<TUser>(this IIdentityServerBuilder builder)
            where TUser : SiteUser
        {
            return builder.AddCloudscribeIdentityServerIntegration<TUser>(AuthenticationScheme.Application);
        }

        public static IIdentityServerBuilder AddCloudscribeIdentityServerIntegration<TUser>(this IIdentityServerBuilder builder, string authenticationScheme)
            where TUser : SiteUser
        {
            builder.Services.AddScoped<IIdentityServerIntegration, Integration>();

            builder.Services.Configure<IdentityServerOptions>(options =>
            {
                options.AuthenticationOptions.AuthenticationScheme = authenticationScheme;
            });

            builder.Services.AddSingleton<IEndpointRouter>(resolver =>
            {
                return new MultiTenantEndpointRouter(CustomConstants.EndpointPathToNameMap,
                    resolver.GetRequiredService<IdentityServerOptions>(),
                    resolver.GetServices<EndpointMapping>(),
                    resolver.GetService<IOptions<MultiTenantOptions>>(),
                    resolver.GetRequiredService<ILogger<MultiTenantEndpointRouter>>());
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Cookies.ApplicationCookie.AuthenticationScheme = authenticationScheme;
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            });

            builder.AddResourceOwnerValidator<ResourceOwnerPasswordValidator<TUser>>();
            builder.Services.AddTransient<IProfileService, ProfileService<TUser>>();

            builder.Services.AddTransient<ISecurityStampValidator, cloudscribe.Core.IdentityServerIntegration.SecurityStampValidator<TUser>>();

            builder.Services.AddScoped<IMatchAuthorizeProtocolRoutePaths, MultiTenantAuthorizeProtocolRouteMatcher>();
            builder.Services.AddScoped<IMatchEndSessionProtocolRoutePaths, MultiTenantEndSessionProtocolRouteMatcher>();

            return builder;
        }

        //public static IServiceCollection AddCloudscribeIdentityServerIntegration(this IServiceCollection services)
        //{
        //    services.AddScoped<IIdentityServerIntegration, Integration>();

        //    return services;
        //}

        public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeIdentityServerIntegration(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(Integration).GetTypeInfo().Assembly,
                    "cloudscribe.Core.IdentityServerIntegration"
                ));

            return options;
        }

    }
}
