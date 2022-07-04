using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerWpf.Library.Models;
public class Category
{
    public int CategoryId { get; set; }
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
}
