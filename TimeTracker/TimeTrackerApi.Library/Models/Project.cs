using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerApi.Library.Models;
public class Project
{
    public int ProjectId { get; set; }
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
    public int? SubcategoryId { get; set; }
    public DateTime DateCreated { get; set; }
}
