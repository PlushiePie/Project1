using System;
using System.Linq;

namespace MyApp.Data;

public static class DbSeed
{
    public static void Seed(AppDbContext db)
    {
        if (db.Teams.Any() || db.Quests.Any() || db.Playthroughs.Any())
            return;

        // 1. Команды
        var team1 = new Team { TeamName = "Химики-любители", MembersCount = 4, IsExperienced = false };
        var team2 = new Team { TeamName = "Менделеевцы", MembersCount = 3, IsExperienced = true };
        var team3 = new Team { TeamName = "Алхимики", MembersCount = 5, IsExperienced = false };
        db.Teams.AddRange(team1, team2, team3);

        // 2. Квесты
        var quest1 = new Quest { QuestTitle = "Утечка в лаборатории", DifficultyLevel = 3, DurationMinutes = 60, ReagentsCount = 5 };
        var quest2 = new Quest { QuestTitle = "Тайна периодической таблицы", DifficultyLevel = 5, DurationMinutes = 90, ReagentsCount = 8 };
        var quest3 = new Quest { QuestTitle = "Синтез антидота", DifficultyLevel = 2, DurationMinutes = 45, ReagentsCount = 3 };
        db.Quests.AddRange(quest1, quest2, quest3);

        db.SaveChanges();

        // 3. Прохождения
        var p1 = new Playthrough { TeamId = team1.Id, QuestId = quest1.Id, StartTime = DateTime.Now.AddDays(-2), IsSuccess = true };
        var p2 = new Playthrough { TeamId = team1.Id, QuestId = quest2.Id, StartTime = DateTime.Now.AddDays(-1), IsSuccess = false };
        var p3 = new Playthrough { TeamId = team2.Id, QuestId = quest1.Id, StartTime = DateTime.Now.AddHours(-5), IsSuccess = true };
        var p4 = new Playthrough { TeamId = team3.Id, QuestId = quest3.Id, StartTime = DateTime.Now.AddHours(-2), IsSuccess = true };

        db.Playthroughs.AddRange(p1, p2, p3, p4);
        db.SaveChanges();
    }
}