using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerWpf.Library.Models;
public class AuthenticatedUser
{
    bool Success { get; set; }
    public string Token { get; set; } = null!;
    public string Username { get; set; } = null!;
}
