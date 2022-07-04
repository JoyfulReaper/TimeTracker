using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerApi.Library.Models;
public class Goal
{
    public int GoalId { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = null!;
}
