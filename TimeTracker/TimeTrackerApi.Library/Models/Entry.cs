using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerApi.Library.Models;
public class Entry
{
    public int EntryId { get; set; }
    public int ProjectId { get; set; }
    public DateTime EntryDate { get; set; }
    public decimal HoursSpent { get; set; }
}
