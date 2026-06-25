namespace BusinessObjects.DTOs;

public class ApiLoginResult
{
    public string Message { get; set; } = string.Empty;
    public LoginResponse User { get; set; } = new();
}
