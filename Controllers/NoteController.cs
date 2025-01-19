using diary_course.Models.DTO;
using diary_course.Services;
using Microsoft.AspNetCore.Mvc;

namespace diary_course.Controllers;

public class NoteController : Controller
{
    private INoteService _noteService;
    private ICategoryService _categoryService;

    public NoteController(INoteService noteService, ICategoryService categoryService)
    {
        _noteService = noteService;
        _categoryService = categoryService;
    }
    
    // 
    [HttpGet]
    public IActionResult Index(int? categoryId)
    {
        // Не авторизован - редирект (выйди и войди нормально)
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }
        // В представлении выводится список записей и категорий
        return View(new NoteListDto(){Categories = _categoryService.getAll(HttpContext.Session.GetInt32("Id")), Notes = _noteService.getNotes(categoryId, HttpContext.Session.GetInt32("Id"))});
    }

    // 
    [HttpGet]
    public IActionResult One(int id)
    {
        // Если не залогинен - на главную
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }

        var note = _noteService.getNote(id);
        // Если заметка не принадлежит пользователю
        if (HttpContext.Session.GetInt32("Id") != note.UserId)
        {
            return RedirectToAction("Index");
        }

        return View(note);
    }

    // Возвращает представление для создания заметки
    [HttpGet]
    public IActionResult Create()
    {
        // Если пользователь не авторизован - на главную
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }
        return View(new AddNoteDto(){Categories = _categoryService.getAll(HttpContext.Session.GetInt32("Id"))});
    }
    
    // Возвращает представление для редактирования заметки
    [HttpGet]
    public IActionResult Edit(int id)
    {
        // Если пользователь не авторизован - на главную
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }
        var note = _noteService.getNote(id);
        // Если пользователю не принадлежит заметка, которую он хочет отредактировать - на главную
        if (HttpContext.Session.GetInt32("Id") != note.UserId)
        {
            return RedirectToAction("Index");
        }
        var categories = _categoryService.getAll(HttpContext.Session.GetInt32("Id"));
        Console.WriteLine(note.Date);
        return View(new UpdateNoteDto() { Id = note.Id, Categories = categories, Date = note.Date, Text = note.Text, CategoryId = note.CategoryId});
    }

    // Создать заметку и вернуться на главную (страница с заметками?)
    [HttpPost]
    public IActionResult Create(AddNoteDto addNoteDto)
    {
        // Не авторизован - редирект
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }

        if (addNoteDto.Date.Year < 1900)
        {
            return View(new AddNoteDto() { Categories = _categoryService.getAll(HttpContext.Session.GetInt32("Id")), Datemiss = true, Text = addNoteDto.Text });
        }
        else
        {
            // Создать заметку с данными из DTOшки для пользователя текущей сессии (UserID)
            _noteService.create(addNoteDto, (int)HttpContext.Session.GetInt32("Id"));
            return RedirectToAction("Index");
        }
    }
    // Обновить данными из DTOшки объект в БД
    [HttpPost]
    public IActionResult Update(UpdateNoteDto updateNoteDto)
    {
        // Пользователь не авторизован
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }
        // Метод обновляет запись, если она есть в БД и у пользователя сесси есть на это право
        _noteService.update(updateNoteDto, (int) HttpContext.Session.GetInt32("Id"));
        return RedirectToAction("Index");
    }
    // Удаление заметки по id
    [HttpGet]
    public IActionResult Delete(int id)
    {
        // Пользователь не авторизован
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return RedirectToAction("Index", "User");
        }
        var note = _noteService.getNote(id);
        // (Пользователь сессии) Не имеет права на удаление
        if (HttpContext.Session.GetInt32("Id") != note.UserId)
        {
            return RedirectToAction("Index");
        }

        _noteService.delete(note.Id);
        return RedirectToAction("Index");
    }
}