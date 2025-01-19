namespace diary_course.Models.DTO;

public class DeleteCategoryDto
{
    public List<Category>? Categories { get; set; }
    public int CategoryId { get; set; }
    public bool? datemiss { get; set; } = false;
}