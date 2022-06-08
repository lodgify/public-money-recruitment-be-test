using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.DataAccess.Contexts;
using VacationRental.Infrastructure.Extensions;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.EntityFramework.Mappers;

const string defaultConnectionParamName = "DefaultConnection";
const string identityServerConnectionParamName = "IdentityServerConnection";
const string identityServerServiceUrlParamName = "IdentityServer:ServiceUrl";

IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true)
                                                          .AddEnvironmentVariables();
IConfiguration configuration = builder.Build();

var services = new ServiceCollection();

var defaultConnection = configuration.GetConnectionString(defaultConnectionParamName);
services.AddDbContext<VacationRentalIdentityServerDbContext>(options => options.UseSqlServer(defaultConnection,
                                                             options => options.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));

var identityServerConnection = configuration.GetConnectionString(identityServerConnectionParamName);
var identityServerServiceUrl = configuration[identityServerServiceUrlParamName];
services.ConfigureIdentityServer(configuration);

using (var serviceProvider = services.BuildServiceProvider())
{
    using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<VacationRentalConfigurationDbContext>();

        foreach (var client in GetClients())
        {
            context.Clients.Add(client.ToEntity());
            context.SaveChanges();
        }

        foreach (var resource in GetIdentityResources())
        {
            context.IdentityResources.Add(resource.ToEntity());
            context.SaveChanges();
        }

        foreach (var api in GetApiResources())
        {
            context.ApiResources.Add(api.ToEntity());
            context.SaveChanges();
        }

        foreach (var apiScope in GetApiScopes())
        {
            context.ApiScopes.Add(apiScope.ToEntity());
            context.SaveChanges();
        }
    }
}

 static IEnumerable<IdentityResource> GetIdentityResources()
{
    return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
}

static IEnumerable<ApiScope> GetApiScopes()
{
    const string name = "api";

    return new List<ApiScope> {
                new ApiScope(name)
            };
}

static IEnumerable<ApiResource> GetApiResources()
{
    const string name = "api";
    const string apiResourceName = "api";
    const string displayName = "Military.IdentityServer";

    var result = new List<ApiResource> {
                new ApiResource
                {
                    Enabled = true,
                    Name = name,
                    DisplayName = displayName,
                    Description = displayName,
                    Scopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        apiResourceName
                    },
                    UserClaims = {
                        JwtClaimTypes.Id,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.EmailVerified,
                        JwtClaimTypes.PhoneNumber,
                        JwtClaimTypes.PhoneNumberVerified,
                        JwtClaimTypes.GivenName,
                        JwtClaimTypes.FamilyName,
                        JwtClaimTypes.PreferredUserName
                    }
                }
            };

    return result;
}

static IEnumerable<Client> GetClients()
{
    const string secret = "secret";
    const string clientId = "default";
    const string apiResourceName = "api";
    const string externalGrantType = "external";

    var allowedGrantTypes = new List<string>();
    allowedGrantTypes.AddRange(GrantTypes.ResourceOwnerPasswordAndClientCredentials);
    allowedGrantTypes.Add(externalGrantType);
    allowedGrantTypes.Add(GrantType.DeviceFlow);

    var result = new List<Client>
            {
                new Client
                {
                    ClientId = clientId,
                    RequireClientSecret = false,
                    AllowedGrantTypes = allowedGrantTypes,
                    AllowOfflineAccess = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 3600,
                    IdentityTokenLifetime = 3600,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    SlidingRefreshTokenLifetime = 30,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AlwaysSendClientClaims = true,
                    Enabled = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    ClientSecrets =  new List<Secret> { new Secret(secret.Sha256()) },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        apiResourceName
                    }
                }
            };

    return result;
}

