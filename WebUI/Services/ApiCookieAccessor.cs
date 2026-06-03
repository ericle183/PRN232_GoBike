namespace WebUI.Services;

public class ApiCookieAccessor : IApiCookieAccessor
{
    private const string SessionKey = "GoBikeApiAuthCookie";
    private readonly IHttpContextAccessor httpContextAccessor;

    public ApiCookieAccessor(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    private ISession? Session => httpContextAccessor.HttpContext?.Session;

    public void SetCookieHeader(string cookieHeader) => Session?.SetString(SessionKey, cookieHeader);

    public string? GetCookieHeader() => Session?.GetString(SessionKey);

    public void Clear() => Session?.Remove(SessionKey);
}
