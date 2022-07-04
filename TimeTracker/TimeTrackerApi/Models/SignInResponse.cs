namespace TimeTrackerApi.Models;

public class SignInResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Username { get; set; }
}
