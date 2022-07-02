using System.ComponentModel.DataAnnotations;

namespace TimeTrackerApi.Models;

public class SignInCredentials
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
