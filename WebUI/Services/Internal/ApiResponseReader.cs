using System.Text.Json;

namespace WebUI.Services.Internal;

internal static class ApiResponseReader
{
    public static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return $"Lỗi API ({(int)response.StatusCode})";

            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("message", out var message))
            {
                var text = message.GetString();
                if (!string.IsNullOrWhiteSpace(text))
                    return text;
            }

            if (doc.RootElement.TryGetProperty("errors", out var errors) &&
                errors.ValueKind == JsonValueKind.Object)
            {
                var messages = new List<string>();
                foreach (var property in errors.EnumerateObject())
                {
                    if (property.Value.ValueKind != JsonValueKind.Array) continue;

                    foreach (var item in property.Value.EnumerateArray())
                    {
                        var text = item.GetString();
                        if (!string.IsNullOrWhiteSpace(text))
                            messages.Add($"{property.Name}: {text}");
                    }
                }

                if (messages.Count > 0)
                    return string.Join(" ", messages);
            }

            if (doc.RootElement.TryGetProperty("title", out var title))
            {
                var text = title.GetString();
                if (!string.IsNullOrWhiteSpace(text))
                    return text;
            }
        }
        catch
        {
            // ignored
        }

        return $"Lỗi API ({(int)response.StatusCode})";
    }
}
