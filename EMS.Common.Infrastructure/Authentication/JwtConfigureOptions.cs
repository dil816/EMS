using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EMS.Common.Infrastructure.Authentication;
internal sealed class JwtConfigureOptions(IConfiguration configuration)
    : IConfigureNamedOptions<JwtBearerOptions>
{
    private const string ConfigurationSectionName = "Authentication";

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }

    public void Configure(JwtBearerOptions options)
    {
        configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
