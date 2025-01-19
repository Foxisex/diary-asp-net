using diary_course.Data;
using diary_course.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������� � ���������.
builder.Services.AddControllersWithViews();

// ��������� �������� ���� ������ DiaryDbContext � ���������.
builder.Services.AddDbContext<DiaryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// ��������� ������ � ���������� ����� 30 ����.
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromDays(30); });

// ��������� ������� �������������, ��������� � ������� � ���������.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INoteService, NoteService>();

var app = builder.Build();
app.UseSession();

// ����������� �������� ��������� HTTP-��������.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ���������� ������� �� ��������� ��� ����������� "Note" � �������� "Index" � ������������ ���������� "categoryId".
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Note}/{action=Index}/{categoryId?}");

app.Run();