using Microsoft.Extensions.DependencyInjection;

namespace EMS.Common.Infrastructure.Authentication;
internal static class AuthenticationExtensions
{
    internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddAuthentication().AddJwtBearer();

        services.AddHttpContextAccessor();

        services.ConfigureOptions<JwtConfigureOptions>();

        return services;
    }
}
