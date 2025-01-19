using diary_course.Data;
using diary_course.Models;
using diary_course.Models.DTO;

namespace diary_course.Services;

public interface INoteService
{
    bool create(AddNoteDto addNoteDto, int userId);
    bool delete(int id);
    bool update(UpdateNoteDto updateNoteDto, int userId);
    List<Note> getNotes(int? categoryId, int? userID);
    Note getNote(int id);
}

public class NoteService : INoteService
{
    private DiaryDbContext _dbContext;

    public NoteService(DiaryDbContext diaryDbContext)
    {
        _dbContext = diaryDbContext;
    }

    // Создать заметку (добавить данные о заметке в БД) для опред. юзера
    public bool create(AddNoteDto addNoteDto, int userId)
    {
        Note note = new Note()
        {
            Text = addNoteDto.Text,
            CategoryId = addNoteDto.CategoryId,
            UserId = userId,
            Date = addNoteDto.Date
        };

        try
        {
            _dbContext.Add(note);
            _dbContext.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    // На вход - id удаляемой заметки
    // На выход - true / false - в зависимости от того - удалена заметка (если была найдена), или нет (нет заметки с таким id для удаления)
    // UserId проверяется в контроллере
    public bool delete(int id)
    {
        var note = _dbContext.Notes.Find(id);

        if (note == null)
        {
            return false;
        }

        _dbContext.Remove(note);
        _dbContext.SaveChanges();

        return true;
    }

    // На вход - данные о заметке для обновления, ID пользователя, чтобы проверить - может ли он обновить заметку
    // Возвращает true / false - в зависимости от того - выполнен update, или нет
    public bool update(UpdateNoteDto updateNoteDto, int userId)
    {
        var note = _dbContext.Notes.Find(updateNoteDto.Id);
        if (note == null)
        {
            return false;
        }

        if (note.UserId != userId)
        {
            return false;
        }

        note.Text = updateNoteDto.Text;
        note.CategoryId = updateNoteDto.CategoryId;
        note.Date = updateNoteDto.Date;

        try
        {
            _dbContext.Update(note);
            _dbContext.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    // На вход - необязательный аргумент - id категории.
    // Возвращает все заметки, если не указана категория, заметки опред. категории, если id указан
    public List<Note> getNotes(int? categoryId, int? userID)
    {
        if (categoryId != null)
        {
            return _dbContext.Notes.Where(n => n.CategoryId == categoryId).ToList();
        }

        return _dbContext.Notes.Where(n => n.UserId == userID).ToList();
    }

    // Возвращает заметку по ее id
    public Note getNote(int id)
    {
        return _dbContext.Notes.Find(id);
    }
}