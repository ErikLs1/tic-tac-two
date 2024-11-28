using ConsoleApp;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;
Menus.MainMenu.Run();
// Console.WriteLine("hello");
// var connectionString = $"Data Source={FileHelper.BasePath + Path.DirectorySeparatorChar}app.db";
//
// var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
//     .UseSqlite(connectionString)
//     .EnableDetailedErrors()
//     .EnableSensitiveDataLogging()
//     .Options;
//
// using var ctx = new AppDbContext(contextOptions);
//
// ctx.Database.Migrate();
//
// Console.WriteLine($"Games in games {ctx.Games.Count()}");
// Console.WriteLine($"Configs in games {ctx.Configurations.Count()}");


