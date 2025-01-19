namespace diary_course.Models;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    
    public ICollection<Note>? Notes { get; set; }
    public ICollection<Category>? Categories { get; set; }
}