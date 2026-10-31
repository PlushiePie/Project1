using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;

namespace MyApp.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Путь к базе данных: {AppDbContext.DbPath}\n");

        using var db = new AppDbContext();

        db.Database.EnsureCreated();
        DbSeed.Seed(db);

        Console.WriteLine("КОМАНДЫ");
        foreach (var team in db.Teams)
        {
            Console.WriteLine(team.ToString());
        }

        Console.WriteLine("\nКВЕСТЫ");
        foreach (var quest in db.Quests)
        {
            Console.WriteLine(quest.ToString());
        }

        Console.WriteLine("\nПРОХОЖДЕНИЯ");
        var playthroughs = db.Playthroughs
            .Include(p => p.Team)
            .Include(p => p.Quest)
            .ToList();

        foreach (var p in playthroughs)
        {
            Console.WriteLine(p.ToString());
        }
    }
}
