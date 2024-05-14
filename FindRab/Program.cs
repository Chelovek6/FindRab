
using FindRab.DataContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

// Добавление сервисов для MVC
builder.Services.AddMvc();

// Настройка аутентификации на основе куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Указание пути для входа
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
    });

// Добавление сервисов авторизации
builder.Services.AddAuthorization();

// Получение строки подключения к базе данных
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BDContext>(options => options.UseSqlServer(connection));

// Добавление сервисов для работы с контроллерами и представлениями
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Настройка маршрутов для контроллеров
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Включение использования аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Включение обслуживания статических файлов
app.UseStaticFiles();

app.Run();



