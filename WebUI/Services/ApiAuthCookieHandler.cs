namespace WebUI.Services;

public class ApiAuthCookieHandler : DelegatingHandler
{
    private readonly IApiCookieAccessor cookieAccessor;

    public ApiAuthCookieHandler(IApiCookieAccessor cookieAccessor)
    {
        this.cookieAccessor = cookieAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var cookieHeader = cookieAccessor.GetCookieHeader();
        if (!string.IsNullOrEmpty(cookieHeader))
            request.Headers.TryAddWithoutValidation("Cookie", cookieHeader);

        return base.SendAsync(request, cancellationToken);
    }
}
