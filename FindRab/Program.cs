
using FindRab.DataContext;
using FindRab.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ���������� �������� ��� MVC
builder.Services.AddMvc();

// ��������� �������������� �� ������ ����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
    });

// ���������� �������� �����������
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireClaim("Role", "1"));
});

// ��������� ������ ����������� � ���� ������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BDContext>(options => options.UseSqlServer(connection));

// ���������� �������� ��� ������ � ������������� � ���������������
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

// ��������� ������������� �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

// Middleware ��� ��������������� � ����������� �� �����
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

// ������������� ���� ������
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BDContext>();
    DbInitializer.Initialize(context);
}

app.Run();
