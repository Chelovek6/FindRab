using FindRab.models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FindRab.DataContext
{
    public static class DbInitializer
    {
        public static void Initialize(BDContext context)
        {
            // Убедитесь, что база данных создана
            context.Database.EnsureCreated();

            // Проверьте, есть ли уже пользователи в базе данных
            if (context.UserM.Any())
            {
                return; // База данных уже инициализирована
            }

            // Создайте нового администратора
            var admin = new User
            {
                
                Username = "Name",
                Password = "123",
                Role = 1
            };

            context.UserM.Add(admin);
            context.SaveChanges();
        }
    }
}