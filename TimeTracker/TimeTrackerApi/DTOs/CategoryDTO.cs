namespace TimeTrackerApi.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime DateCreated { get; set; }

    public int ProjectsInCategory { get; set; }
}
