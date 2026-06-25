namespace WebUI.Services;

public interface IApiCookieAccessor
{
    void SetCookieHeader(string cookieHeader);
    string? GetCookieHeader();
    void Clear();
}
