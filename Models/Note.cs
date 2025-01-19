using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diary_course.Models;

public class Note
{
    public int Id { get; set; }
    public string? Text { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    [Required]
    public DateOnly Date { get; set; }
}
