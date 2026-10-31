using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace MyApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Team> Teams { get; set; }
    public DbSet<Quest> Quests { get; set; }
    public DbSet<Playthrough> Playthroughs { get; set; }

    public static string DbPath
    {
        get
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, "myapp.db");
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}