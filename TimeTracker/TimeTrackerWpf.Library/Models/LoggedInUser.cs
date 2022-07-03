using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerWpf.Library.Models;
public class LoggedInUser : ILoggedInUser
{
    public string UserId { get; set; } = null!;
    public string? Token { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
}
