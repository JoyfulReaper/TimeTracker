using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerApi.Library.Models;
public class ToDoListItem
{
    public int ToDoListItemId { get; set; }
    public int ToDoListId { get; set; }
    public string Name { get; set; } = null!;
    public bool Completed { get; set; }
}
