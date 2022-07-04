namespace TimeTrackerApi.DTOs;

public class SubcategoryDTO
{
    public int SubcategoryId { get; set; }
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
    public DateTime DateCreated { get; set; }

    public int NumberOfProjectsInSubcategory { get; set; }
}
