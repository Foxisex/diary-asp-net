using diary_course.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;

namespace diary_course.Data;

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
	public DateOnlyConverter() : base(
		dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
		dateTime => DateOnly.FromDateTime(dateTime))
	{ }
}

public class DiaryDbContext : DbContext
{
    public DiaryDbContext(DbContextOptions options) : base(options)
    {
        
    }

	protected override void ConfigureConventions(ModelConfigurationBuilder builder)
	{
		base.ConfigureConventions(builder);
		builder.Properties<DateOnly>()
			.HaveConversion<DateOnlyConverter>();
	}

	public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>().HasOne(n => n.Category).WithMany(c => c.Notes).HasForeignKey(n => n.CategoryId);
        modelBuilder.Entity<Note>().HasOne(n => n.User).WithMany(u => u.Notes).HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Category>().HasOne(c => c.User).WithMany(u => u.Categories).HasForeignKey(c => c.UserId);
    }

}