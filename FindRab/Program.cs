
using FindRab.DataContext;
using FindRab.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов для MVC
builder.Services.AddMvc();

// Настройка аутентификации на основе куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
    });

// Добавление сервисов авторизации
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireClaim("Role", "1"));
});

// Получение строки подключения к базе данных
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BDContext>(options => options.UseSqlServer(connection));

// Добавление сервисов для работы с контроллерами и представлениями
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Включение использования аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Middleware для перенаправления в зависимости от ролей
app.UseMiddleware<RoleRedirectMiddleware>();

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=AdminView}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "user",
    pattern: "User/{controller=Menu}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Menu}/{action=Index}/{id?}");

app.Run();
