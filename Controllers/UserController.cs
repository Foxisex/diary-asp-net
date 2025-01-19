using diary_course.Models.DTO;
using diary_course.Services;
using Microsoft.AspNetCore.Mvc;

namespace diary_course.Controllers;

public class UserController : Controller
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(AuthUserDto authUserDto)
    {
        if (HttpContext.Session.GetInt32("Id") != null)
        {
            return RedirectToAction("Index", "Note");
        }
        var user = _userService.login(authUserDto);
        if (user == null)
        {
            return RedirectToAction("Index");
        }
        
        HttpContext.Session.SetInt32("Id", user.Id);
        //HttpContext.Session.Remove("Id");
        return RedirectToAction("Index", "Note");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("Id");
        return RedirectToAction("Index", "User");
    }

    [HttpPost]
    public IActionResult Register(AuthUserDto authUserDto)
    {
        if (HttpContext.Session.GetInt32("Id") != null)
        {
            return RedirectToAction("Index", "Note");
        }
        var user = _userService.create(authUserDto);
        if (user == null)
        {
            return RedirectToAction("Register");
        }
        HttpContext.Session.SetInt32("Id", user.Id);
        return RedirectToAction("Index", "Note");
    }
    
    
}