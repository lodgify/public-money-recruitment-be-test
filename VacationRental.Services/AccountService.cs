using IdentityModel;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using VacationRental.Models.Configurations;
using VacationRental.Models.Dtos;
using VacationRental.Services.Interfaces;

namespace VacationRental.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDeviceAuthorizationResponseGenerator _deviceAuthorizationResponseGenerator;
        private readonly IdentityServerConfiguration _identityServerConfiguration;
        private readonly ITokenResponseGenerator _tokenResponseGenerator;
        private readonly IDeviceFlowCodeService _deviceFlowCodeService;
        private readonly IDeviceFlowStore _deviceFlowStore;
        private readonly IClientStore _clientStore;

        public AccountService(IDeviceAuthorizationResponseGenerator deviceAuthorizationResponseGenerator,
                              IOptions<IdentityServerConfiguration> identityServerConfigurationOptions,
                              ITokenResponseGenerator tokenResponseGenerator,
                              IDeviceFlowCodeService deviceFlowCodeService,
                              IDeviceFlowStore deviceFlowStore,
                              IClientStore clientStore)
        {
            _deviceAuthorizationResponseGenerator = deviceAuthorizationResponseGenerator;
            _identityServerConfiguration = identityServerConfigurationOptions.Value;
            _tokenResponseGenerator = tokenResponseGenerator;
            _deviceFlowCodeService = deviceFlowCodeService;
            _deviceFlowStore = deviceFlowStore;
            _clientStore = clientStore;
        }

        public async Task<AccessTokenDto> SignInGuestAsync()
        {
            const string defaultClientId = "default";

            var client = await _clientStore.FindClientByIdAsync(defaultClientId);
            client.ClientClaimsPrefix = string.Empty;

            var deviceAuthorizationResponse = await _deviceAuthorizationResponseGenerator.ProcessAsync(new IdentityServer4.Validation.DeviceAuthorizationRequestValidationResult(new IdentityServer4.Validation.ValidatedDeviceAuthorizationRequest
            {
                Client = client
            }), _identityServerConfiguration.ServiceUrl);

            var deviceCode = await _deviceFlowCodeService.FindByUserCodeAsync(deviceAuthorizationResponse.UserCode);

            deviceCode.AuthorizedScopes = client.AllowedScopes;
            deviceCode.RequestedScopes = client.AllowedScopes;

            await _deviceFlowStore.StoreDeviceAuthorizationAsync(deviceAuthorizationResponse.DeviceCode, deviceAuthorizationResponse.UserCode, new IdentityServer4.Models.DeviceCode
            {
                AuthorizedScopes = client.AllowedScopes,
                RequestedScopes = client.AllowedScopes,
                ClientId = client.ClientId,
                Description = client.Description,
                IsAuthorized = deviceCode.IsAuthorized,
                IsOpenId = deviceCode.IsOpenId,
                Lifetime = deviceCode.Lifetime,
                SessionId = deviceCode.SessionId,
                Subject = deviceCode.Subject,
                CreationTime = deviceCode.CreationTime
            });

            const string userCodeClaimName = "userCode";
            const string deviceCodeClaimName = "deviceCode";
            var token = await _tokenResponseGenerator.ProcessAsync(new IdentityServer4.Validation.TokenRequestValidationResult(new IdentityServer4.Validation.ValidatedTokenRequest
            {
                GrantType = OidcConstants.GrantTypes.DeviceCode,
                AccessTokenLifetime = client.AccessTokenLifetime,
                AccessTokenType = IdentityServer4.Models.AccessTokenType.Jwt,
                UserName = deviceAuthorizationResponse.UserCode,
                RequestedScopes = client.AllowedScopes,
                Client = client,
                DeviceCode = deviceCode,
                ClientId = client.ClientId,
                ClientClaims = new List<Claim> {
                    new Claim(JwtClaimTypes.Subject, deviceAuthorizationResponse.UserCode),
                    new Claim(userCodeClaimName, deviceAuthorizationResponse.UserCode),
                    new Claim(deviceCodeClaimName, deviceAuthorizationResponse.DeviceCode)
                }
            }));

            var result = new AccessTokenDto
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                ExpiresIn = token.AccessTokenLifetime,
                TokenType = JwtBearerDefaults.AuthenticationScheme
            };

            return result;
        }
    }
}
