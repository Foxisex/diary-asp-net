using System.ComponentModel.DataAnnotations;

namespace diary_course.Models.DTO;

public class AddNoteDto
{
    public string Text { get; set; }
    public List<Category> Categories { get; set; }
    public int CategoryId { get; set; }
    [Required]
    public DateOnly Date { get; set; }
    public bool Datemiss { get; set; } = false;
}