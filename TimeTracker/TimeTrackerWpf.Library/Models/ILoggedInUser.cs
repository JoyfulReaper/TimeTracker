namespace TimeTrackerWpf.Library.Models;

public interface ILoggedInUser
{
    string UserId { get; set; }
    string EmailAddress { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string? Token { get; set; }
}