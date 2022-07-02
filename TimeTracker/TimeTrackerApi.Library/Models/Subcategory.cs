using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerApi.Library.Models;
public class Subcategory
{
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
    public DateTime DateCreated { get; set; }
}
