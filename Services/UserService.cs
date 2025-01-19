using diary_course.Data;
using diary_course.Models;
using diary_course.Models.DTO;

namespace diary_course.Services;

public interface IUserService
{
    User? create(AuthUserDto authUserDto);
    User? login(AuthUserDto authUserDto);
}

public class UserService : IUserService
{
    private DiaryDbContext _dbContext;

    public UserService(DiaryDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public User? create(AuthUserDto authUserDto)
    {
        User user = new User()
        {
            Username = authUserDto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(authUserDto.Password)
        };

        try
        {
            _dbContext.Add(user);
            _dbContext.SaveChanges();
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public User? login(AuthUserDto authUserDto)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == authUserDto.Username);

        if (user == null)
        {
            return null;
        }

        bool passwordIsValid = BCrypt.Net.BCrypt.Verify(authUserDto.Password, user.Password);
        if (!passwordIsValid)
        {
            return null;
        }

        return user;
    }
}