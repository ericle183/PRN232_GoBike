using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BusinessObjects.DTOs;
using Microsoft.Extensions.Options;
using WebUI.Configuration;
using WebUI.Services.Internal;

namespace WebUI.Services;

public class GoBikeApiClient : IGoBikeApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    private readonly HttpClient httpClient;
    private readonly IApiCookieAccessor cookieAccessor;
    private readonly ApiSettings apiSettings;

    public GoBikeApiClient(
        HttpClient httpClient,
        IApiCookieAccessor cookieAccessor,
        IOptions<ApiSettings> apiSettings)
    {
        this.httpClient = httpClient;
        this.cookieAccessor = cookieAccessor;
        this.apiSettings = apiSettings.Value;
    }

    public async Task<(bool Success, LoginResponse? User, string? Error)> LoginAsync(LoginRequest request)
    {
        cookieAccessor.Clear();

        var baseUri = new Uri(apiSettings.BaseUrl.TrimEnd('/') + "/");
        var handler = new HttpClientHandler { UseCookies = true, CookieContainer = new CookieContainer() };
        using var loginClient = new HttpClient(handler) { BaseAddress = baseUri };

        var response = await loginClient.PostAsJsonAsync("api/auth/login", request, JsonOptions);
        if (!response.IsSuccessStatusCode)
            return (false, null, await ApiResponseReader.ReadErrorMessageAsync(response));

        var cookies = handler.CookieContainer.GetCookies(baseUri);
        if (cookies.Count > 0)
        {
            var cookieHeader = string.Join("; ", cookies.Cast<Cookie>().Select(c => $"{c.Name}={c.Value}"));
            cookieAccessor.SetCookieHeader(cookieHeader);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiLoginResult>(JsonOptions);
        if (result?.User == null)
            return (false, null, "Invalid response from API");

        return (true, result.User, null);
    }

    public async Task LogoutAsync()
    {
        try
        {
            if (!string.IsNullOrEmpty(cookieAccessor.GetCookieHeader()))
                await httpClient.PostAsync("api/auth/logout", null);
        }
        finally
        {
            cookieAccessor.Clear();
        }
    }

    public async Task<(bool Success, UserProfileDto? Profile, string? Error)> GetProfileAsync()
    {
        var response = await httpClient.GetAsync("api/auth/profile");
        return await ReadAsync<UserProfileDto>(response);
    }

    public async Task<(bool Success, List<UserDto>? Users, string? Error)> GetStaffUsersAsync()
    {
        var response = await httpClient.GetAsync("api/users/staff");
        return await ReadAsync<List<UserDto>>(response);
    }

    public async Task<(bool Success, UserDto? User, string? Error)> GetStaffUserAsync(int id)
    {
        var response = await httpClient.GetAsync($"api/users/staff/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return (false, null, "Staff user not found");
        return await ReadAsync<UserDto>(response);
    }

    public async Task<(bool Success, UserDto? User, string? Error)> CreateStaffUserAsync(CreateStaffUserRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/users/staff", request, JsonOptions);
        return await ReadAsync<UserDto>(response);
    }

    public async Task<(bool Success, UserDto? User, string? Error)> UpdateStaffUserAsync(int id, UpdateStaffUserRequest request)
    {
        var response = await httpClient.PutAsJsonAsync($"api/users/staff/{id}", request, JsonOptions);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return (false, null, "Staff user not found");
        return await ReadAsync<UserDto>(response);
    }

    public async Task<(bool Success, string? Error)> DeleteStaffUserAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"api/users/staff/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return (false, "Staff user not found");
        if (!response.IsSuccessStatusCode)
            return (false, await ApiResponseReader.ReadErrorMessageAsync(response));
        return (true, null);
    }

    private static async Task<(bool Success, T? Data, string? Error)> ReadAsync<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return (false, default, "Phiên API đã hết hạn. Vui lòng đăng nhập lại.");

        if (!response.IsSuccessStatusCode)
            return (false, default, await ApiResponseReader.ReadErrorMessageAsync(response));

        var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        return (true, data, null);
    }
}
