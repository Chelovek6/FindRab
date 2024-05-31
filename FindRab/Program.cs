
using FindRab.DataContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

// ���������� �������� ��� MVC
builder.Services.AddMvc();

// ��������� �������������� �� ������ ����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // �������� ���� ��� �����
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
    });

// ���������� �������� �����������
builder.Services.AddAuthorization();
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

// ��������� ��������� ��� ������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Menu}/{action=Index}/{id?}");


// ��������� ������������� �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

// ��������� ������������ ����������� ������
app.UseStaticFiles();

app.Run();



