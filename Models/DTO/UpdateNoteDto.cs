namespace diary_course.Models.DTO;

public class UpdateNoteDto
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public List<Category>? Categories { get; set; }
    public int CategoryId { get; set; }
    public DateOnly Date { get; set; }
}