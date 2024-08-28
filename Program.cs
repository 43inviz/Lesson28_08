using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Lesson__2__28_08
{
    internal class Program
    {
        static void Main()
        {
            using (ApplicationContext db = new ApplicationContext())
            {

                db.Database.EnsureCreated();

                db.Users.Add(new User
                {
                    Name = "Tom",
                    age = 30
                });
                db.SaveChanges();


                var allUsers = db.Users.Select(e => new User
                {
                    Name = e.Name
                }).ToList();
                



                //db.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());//




            }
        }

    }
    public class User { 
        public int Id { get; set; } 
        public string Name { get; set; }
        public int age { get; set; }


    
    }

    public class ApplicationContext :  DbContext {
        private readonly StreamWriter writer = new StreamWriter("log.txt", true);

        public DbSet<User> Users { get; set; } = null;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = DESKTOP-R3LQDV9; Database = testDB1; Trusted_Connection = True; TrustServerCertificate = True");

            ///ЛОГИРОВАНИЕ
            optionsBuilder.LogTo(writer.WriteLine);//loger в файл

            //optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Error);//logger
            //optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name });//logger в консоли выводит имя команды

            //bests4use
            //optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });//выводит консоли только отправленные запросы

        }

    }
}


public class MyLoggerProvider : ILoggerProvider //шаблон продвинутого логгера
{
    public ILogger CreateLogger(string categoryName)
    {
        return new MyLogger();
    }

    public void Dispose()
    {

    }
    private class MyLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Console.WriteLine(formatter(state, exception));
            File.AppendAllText("log.txt", formatter(state, exception));
        }
    }
}