using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mine.Commerce.Identity.Data;

namespace Mine.Commerce.Identity.Areas.Identity.StartupExtension
{
    public static class StartupExtension
    {
        private static void CleanClients(ConfigurationDbContext context){
            foreach (var client in context.Clients)
            {
                context.Clients.Remove(client);
            }
            foreach (var identityResource in context.IdentityResources)
            {
                context.IdentityResources.Remove(identityResource);
            }
             foreach (var apiResource in context.ApiResources)
            {
                context.ApiResources.Remove(apiResource);
            }
            context.SaveChanges();
        }

        private static void CleanUser(ApplicationDbContext context){
            foreach (var client in context.Users)
            {
                context.Users.Remove(client);
            }

            foreach (var identityResource in context.UserRoles)
            {
                context.UserRoles.Remove(identityResource);
            }

            foreach (var identityResource in context.Roles)
            {
                context.Roles.Remove(identityResource);
            }

             foreach (var userClaim in context.UserClaims)
             {
                context.UserClaims.Remove(userClaim);
            }
            // context.UserClaims.Add(new IdentityUserClaim<string>
            // {
            //     ClaimType = "Role",
            //     ClaimValue = "Admin",

            // })
            context.SaveChanges();
        }

        public static void InitializeDatabase(this IApplicationBuilder app, IConfiguration configuration)
        {
            var clientUrls = new Dictionary<string, string>
            {
                ["Web"] = configuration["ClientUrl:Web"],
                ["Admin-web"] = configuration["ClientUrl:Admin_web"],
                ["Swagger"] = configuration["ClientUrl:Swagger"],
                ["Idp"] = configuration["ClientUrl:Idp"],
                ["Service"] = configuration["ClientUrl:Service"]
            };
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var persistantContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                CleanClients(context);
                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityServerConfig.Clients(clientUrls))
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityServerConfig.Ids)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityServerConfig.Apis)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                //CleanUser(applicationDbContext);
                if (!applicationDbContext.Users.Any())
                {
                    var adminRole = new IdentityRole("Admin")
                    {
                        Id = Guid.NewGuid().ToString(),
                        NormalizedName = "admin".ToUpper(),
                        
                    };
                    var userRole = new IdentityRole("User")
                    {
                        Id = Guid.NewGuid().ToString(),
                        NormalizedName = "user".ToUpper()
                    };
                    var roles = new List<IdentityRole>
                    {
                        adminRole, userRole
                    };

                    applicationDbContext.Roles.AddRange(roles);
                    applicationDbContext.SaveChanges();

                    var passwordHasher = new PasswordHasher<IdentityUser>();
                    var user = new IdentityUser("admin@minecommerce.com");
                    user.Id = Guid.NewGuid().ToString();
                    user.Email = "admin@minecommerce.com";
                    user.NormalizedEmail = "admin@minecommerce.com".ToUpper();
                    user.NormalizedUserName = "admin@minecommerce.com".ToUpper();
                    user.EmailConfirmed = true;
                    user.PasswordHash = passwordHasher.HashPassword(user, "Ch@ngeMe");
                    user.AccessFailedCount = 0;

                    applicationDbContext.Users.Add(user);
                    applicationDbContext.SaveChanges();

                    applicationDbContext.UserRoles.Add(new IdentityUserRole<string>
                    {
                        UserId = user.Id,
                        RoleId = adminRole.Id
                    });
                    applicationDbContext.SaveChanges();
                }
            }
        }
    }
}