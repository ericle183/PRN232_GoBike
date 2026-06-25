using WebUI.Configuration;
using WebUI.Services;

namespace WebUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoBikeApiClient(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.Configure<ApiSettings>(configuration.GetSection(ApiSettings.SectionName));
        services.AddScoped<IApiCookieAccessor, ApiCookieAccessor>();
        services.AddTransient<ApiAuthCookieHandler>();

        var httpClientBuilder = services.AddHttpClient<IGoBikeApiClient, GoBikeApiClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(30);
        }).AddHttpMessageHandler<ApiAuthCookieHandler>();

        if (environment.IsDevelopment())
        {
            httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
        }

        return services;
    }
}
