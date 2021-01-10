using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Mine.Commerce.Identity
{
    public class IdentityServerConfig
    {
        private const string InternalScope = "api.minecommerce";
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource("role", new List<string>{"role"})            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("MineCommerceAPI", "MineCommerce API", new List<string>{"Role"} )
                    {
                        Scopes = new List<string>
                        {
                            InternalScope
                        },
                        ApiSecrets = new List<Secret>{
                            new Secret("secret".Sha256())
                        }
                    }
            };

        public static IEnumerable<Client> Clients(Dictionary<string, string> clientUrls) =>
            new []
            {
                new Client
                {
                    ClientId = "ro.client",
                    ClientName = "Resource Owner Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { InternalScope },
                    ClientUri = ""
                },
                new Client
                {
                    ClientId = "Web",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true,
                    AllowRememberConsent = true,
                    AlwaysSendClientClaims = true,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                    AllowOfflineAccess = true,
                    ClientUri = clientUrls["Web"],
                    RedirectUris = { $"{clientUrls["Web"]}/signin-oidc",  },
                    PostLogoutRedirectUris = { $"{clientUrls["Web"]}/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        InternalScope
                    }
                 },
                new Client
                {
                    ClientId = "Service",
                    ClientSecrets = { new Secret("secret".Sha256())  },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,
                    AccessTokenType = AccessTokenType.Reference,
                    RequireConsent = false,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true,
                    AllowRememberConsent = true,
                    AlwaysSendClientClaims = true,
                    RedirectUris = { $"{clientUrls["Service"]}/authentication/login-callback" },
                    PostLogoutRedirectUris = { $"{clientUrls["Service"]}/" },
                    AllowedCorsOrigins = new List<string> { $"{clientUrls["Service"]}" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        InternalScope
                    }
                 },
                new Client
                {
                    ClientId = "swagger",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,

                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris =           { $"{clientUrls["Swagger"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["Swagger"]}/swagger/oauth2-redirect.html" },
                    AllowedCorsOrigins =     { $"{clientUrls["Swagger"]}" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        InternalScope
                    }
                },
                new Client
                {
                    ClientName = "Idp",
                    ClientId = "Idp",
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,

                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris = new List<string>
                    {
                        $"{clientUrls["Idp"]}/authentication/login-callback",
                        $"{clientUrls["Idp"]}/silent-renew.html",
                        $"{clientUrls["Idp"]}"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{clientUrls["Idp"]}/unauthorized",
                        $"{clientUrls["Idp"]}/authentication/logout-callback",
                        $"{clientUrls["Idp"]}"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        $"{clientUrls["Idp"]}"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        InternalScope
                    }
                },
                new Client
                {
                    ClientId = "Admin-web",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowRememberConsent = true,
                    AlwaysSendClientClaims = true,
                    RequireConsent = false,
                    RequirePkce = true,
                    AccessTokenType = AccessTokenType.Reference,
                    RequireClientSecret = true,
                    RedirectUris = { 
                        $"{clientUrls["Admin-web"]}/signin-oidc",
                        $"{clientUrls["Admin-web"]}"
                        },
                    PostLogoutRedirectUris = { $"{clientUrls["Admin-web"]}/signout-callback-oidc" },
                    AllowedCorsOrigins = new List<string> { $"{clientUrls["Admin-web"]}"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        InternalScope
                    },
                    
                 }
            };
    }
}