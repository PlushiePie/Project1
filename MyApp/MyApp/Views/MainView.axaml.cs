using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using System.Linq;

namespace MyApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        LoadData();
    }

    private void LoadData()
    {
        using var db = new AppDbContext();

        db.Database.EnsureCreated();
        DbSeed.Seed(db);

        TeamsList.ItemsSource = db.Teams.ToList();

        QuestsList.ItemsSource = db.Quests.ToList();

        PlaythroughsList.ItemsSource = db.Playthroughs
            .Include(p => p.Team)
            .Include(p => p.Quest)
            .ToList();
    }
}