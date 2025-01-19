using diary_course.Models.DTO;
using diary_course.Services;
using Microsoft.AspNetCore.Mvc;

namespace diary_course.Controllers;

public class CategoryController : Controller
{
    private ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // Вернуть представление для создания категории
    [HttpGet]
    public IActionResult Create()
    {
        // Пользователь не авторизован
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }

        return View();
    }

    // Создать категорию с данными из DTO
    [HttpPost]
    public IActionResult Create(AddCategoryDto addCategoryDto)
    {
        // Пользователь не авторизован
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }
        // Создать категорию для пользователя сессии
        _categoryService.create(addCategoryDto, (int)HttpContext.Session.GetInt32("Id"));
        return RedirectToAction("Index", "Note");
    }

    [HttpGet]
    public IActionResult Delete()
    {
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }

        var categories = _categoryService.getAll(HttpContext.Session.GetInt32("Id"));
        return View(new DeleteCategoryDto {Categories = categories });
    }

    // Удалить категорию
    [HttpPost]
    public IActionResult Deletee(DeleteCategoryDto cat)
    {
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }
        _categoryService.delete(cat.CategoryId, (int) HttpContext.Session.GetInt32("Id"));
        return RedirectToAction("Index", "Note");
    }
}