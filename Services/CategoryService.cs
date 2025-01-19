using diary_course.Data;
using diary_course.Models;
using diary_course.Models.DTO;

namespace diary_course.Services;

public interface ICategoryService
{
    bool create(AddCategoryDto addCategoryDto, int userId);
    bool delete(int id, int userId);
    List<Category> getAll(int? userID);
}

public class CategoryService : ICategoryService
{
    private DiaryDbContext _dbContext;

    public CategoryService(DiaryDbContext diaryDbContext)
    {
        _dbContext = diaryDbContext;
    }

    // Создание категории заметок для пользователя
    // Возвращает - true / false (добавил / не добавил в БД)
    public bool create(AddCategoryDto addCategoryDto, int userId)
    {
        var category = new Category()
        {
            Name = addCategoryDto.Name,
            UserId = userId
        };

        try
        {
            _dbContext.Add(category);
            _dbContext.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }        
        
    }

    // Удаление категории по id с проверкой - имеет ли право пользователь на удаление категории с переданным id
    public bool delete(int id, int userId)
    {
        var category = _dbContext.Categories.Find(id);
        // Если не нашел категорию с таким id
        if (category == null)
        {
            return false;
        }

        if (category.UserId != userId)
        {
            return false;
        }

        _dbContext.Remove(category);
        _dbContext.SaveChanges();

        return true;
    }

    // Получить список категорий
    public List<Category> getAll(int? userID)
    {
        return _dbContext.Categories.Where(n => n.UserId == userID).ToList();
    }
}