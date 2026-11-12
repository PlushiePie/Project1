using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using System;
using System.Linq;

namespace MyApp.Views;

public partial class MainView : UserControl
{
    private int? _selectedTeamId;
    private int? _selectedQuestId;
    private int? _selectedPlaythroughId;

    public MainView()
    {
        InitializeComponent();
        LoadAllData();
    }

    private void LoadAllData()
    {
        try
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
            DbSeed.Seed(db);

            TeamsList.ItemsSource = db.Teams.ToList();
            QuestsList.ItemsSource = db.Quests.ToList();

            var playthroughs = db.Playthroughs
                .Include(p => p.Team)
                .Include(p => p.Quest)
                .ToList();
            PlaythroughsList.ItemsSource = playthroughs;

            PlaythroughTeamCombo.ItemsSource = db.Teams.ToList();
            PlaythroughTeamCombo.DisplayMemberBinding = new Avalonia.Data.Binding("TeamName");

            PlaythroughQuestCombo.ItemsSource = db.Quests.ToList();
            PlaythroughQuestCombo.DisplayMemberBinding = new Avalonia.Data.Binding("QuestTitle");
        }
        catch (Exception ex)
        {
            PlaythroughMessage.Text = $"Ошибка загрузки данных: {ex.Message}";
        }
    }

    private void TeamsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (TeamsList.SelectedItem is Team team)
        {
            _selectedTeamId = team.Id;
            TeamNameBox.Text = team.TeamName;
            TeamMembersBox.Text = team.MembersCount.ToString();
            TeamIsExperiencedBox.IsChecked = team.IsExperienced;
        }
    }

    private void AddTeam_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TeamNameBox.Text) || !int.TryParse(TeamMembersBox.Text, out int members))
        {
            TeamMessage.Text = "Ошибка: Проверьте название и количество человек (число).";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            db.Teams.Add(new Team { TeamName = TeamNameBox.Text, MembersCount = members, IsExperienced = TeamIsExperiencedBox.IsChecked ?? false });
            db.SaveChanges();
            TeamMessage.Text = "Команда добавлена!";
            LoadAllData();
        }
        catch (Exception ex) { TeamMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void SaveTeam_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedTeamId == null) { TeamMessage.Text = "Выберите команду для изменения."; return; }
        if (string.IsNullOrWhiteSpace(TeamNameBox.Text) || !int.TryParse(TeamMembersBox.Text, out int members))
        {
            TeamMessage.Text = "Ошибка: Проверьте данные.";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var team = db.Teams.Find(_selectedTeamId);
            if (team != null)
            {
                team.TeamName = TeamNameBox.Text;
                team.MembersCount = members;
                team.IsExperienced = TeamIsExperiencedBox.IsChecked ?? false;
                db.SaveChanges();
                TeamMessage.Text = "Команда сохранена!";
                LoadAllData();
            }
        }
        catch (Exception ex) { TeamMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void DeleteTeam_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedTeamId == null) { TeamMessage.Text = "Выберите команду для удаления."; return; }

        try
        {
            using var db = new AppDbContext();
            var team = db.Teams.Find(_selectedTeamId);
            if (team != null)
            {
                db.Teams.Remove(team);
                db.SaveChanges();
                TeamMessage.Text = "Команда удалена!";
                _selectedTeamId = null;
                LoadAllData();
            }
        }
        catch (Exception ex) { TeamMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void QuestsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (QuestsList.SelectedItem is Quest quest)
        {
            _selectedQuestId = quest.Id;
            QuestTitleBox.Text = quest.QuestTitle;
            QuestDifficultyBox.Text = quest.DifficultyLevel.ToString();
            QuestDurationBox.Text = quest.DurationMinutes.ToString();
            QuestReagentsBox.Text = quest.ReagentsCount.ToString();
        }
    }

    private void AddQuest_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(QuestTitleBox.Text) ||
            !int.TryParse(QuestDifficultyBox.Text, out int diff) ||
            !int.TryParse(QuestDurationBox.Text, out int dur) ||
            !int.TryParse(QuestReagentsBox.Text, out int reag))
        {
            QuestMessage.Text = "Ошибка: Проверьте ввод (все числовые поля должны быть числами).";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            db.Quests.Add(new Quest { QuestTitle = QuestTitleBox.Text, DifficultyLevel = diff, DurationMinutes = dur, ReagentsCount = reag });
            db.SaveChanges();
            QuestMessage.Text = "Квест добавлен!";
            LoadAllData();
        }
        catch (Exception ex) { QuestMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void SaveQuest_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedQuestId == null) { QuestMessage.Text = "Выберите квест для изменения."; return; }
        if (string.IsNullOrWhiteSpace(QuestTitleBox.Text) ||
            !int.TryParse(QuestDifficultyBox.Text, out int diff) ||
            !int.TryParse(QuestDurationBox.Text, out int dur) ||
            !int.TryParse(QuestReagentsBox.Text, out int reag))
        {
            QuestMessage.Text = "Ошибка: Проверьте ввод.";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var quest = db.Quests.Find(_selectedQuestId);
            if (quest != null)
            {
                quest.QuestTitle = QuestTitleBox.Text;
                quest.DifficultyLevel = diff;
                quest.DurationMinutes = dur;
                quest.ReagentsCount = reag;
                db.SaveChanges();
                QuestMessage.Text = "Квест сохранен!";
                LoadAllData();
            }
        }
        catch (Exception ex) { QuestMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void DeleteQuest_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedQuestId == null) { QuestMessage.Text = "Выберите квест для удаления."; return; }

        try
        {
            using var db = new AppDbContext();
            var quest = db.Quests.Find(_selectedQuestId);
            if (quest != null)
            {
                db.Quests.Remove(quest);
                db.SaveChanges();
                QuestMessage.Text = "Квест удален!";
                _selectedQuestId = null;
                LoadAllData();
            }
        }
        catch (Exception ex) { QuestMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void PlaythroughsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (PlaythroughsList.SelectedItem is Playthrough p)
        {
            _selectedPlaythroughId = p.Id;

            var team = PlaythroughTeamCombo.ItemsSource?.Cast<Team>().FirstOrDefault(t => t.Id == p.TeamId);
            var quest = PlaythroughQuestCombo.ItemsSource?.Cast<Quest>().FirstOrDefault(q => q.Id == p.QuestId);

            PlaythroughTeamCombo.SelectedItem = team;
            PlaythroughQuestCombo.SelectedItem = quest;

            PlaythroughDatePicker.SelectedDate = p.StartTime.Date;
            PlaythroughTimePicker.SelectedTime = p.StartTime.TimeOfDay;
            PlaythroughIsSuccessBox.IsChecked = p.IsSuccess;

            PlaythroughMessage.Text = "";
        }
    }

    private void AddPlaythrough_Click(object? sender, RoutedEventArgs e)
    {
        if (PlaythroughTeamCombo.SelectedItem is not Team selectedTeam ||
            PlaythroughQuestCombo.SelectedItem is not Quest selectedQuest ||
            PlaythroughDatePicker.SelectedDate == null)
        {
            PlaythroughMessage.Text = "Ошибка: Выберите команду, квест и дату.";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var time = PlaythroughTimePicker.SelectedTime ?? TimeSpan.Zero;
            var dateTime = PlaythroughDatePicker.SelectedDate.Value.Date + time;

            var p = new Playthrough
            {
                TeamId = selectedTeam.Id,
                QuestId = selectedQuest.Id,
                StartTime = dateTime,
                IsSuccess = PlaythroughIsSuccessBox.IsChecked ?? false
            };

            db.Playthroughs.Add(p);
            db.SaveChanges();
            PlaythroughMessage.Text = "Прохождение добавлено!";
            LoadAllData();
        }
        catch (Exception ex) { PlaythroughMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void SavePlaythrough_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedPlaythroughId == null) { PlaythroughMessage.Text = "Выберите прохождение для изменения."; return; }
        if (PlaythroughTeamCombo.SelectedItem is not Team selectedTeam ||
            PlaythroughQuestCombo.SelectedItem is not Quest selectedQuest ||
            PlaythroughDatePicker.SelectedDate == null)
        {
            PlaythroughMessage.Text = "Ошибка: Выберите команду, квест и дату.";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var p = db.Playthroughs.Find(_selectedPlaythroughId);
            if (p != null)
            {
                p.TeamId = selectedTeam.Id;
                p.QuestId = selectedQuest.Id;
                var time = PlaythroughTimePicker.SelectedTime ?? TimeSpan.Zero;
                p.StartTime = PlaythroughDatePicker.SelectedDate.Value.Date + time;
                p.IsSuccess = PlaythroughIsSuccessBox.IsChecked ?? false;

                db.SaveChanges();
                PlaythroughMessage.Text = "Прохождение сохранено!";
                _selectedPlaythroughId = null;
                LoadAllData();
            }
        }
        catch (Exception ex) { PlaythroughMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void DeletePlaythrough_Click(object? sender, RoutedEventArgs e)
    {
        if (PlaythroughsList.SelectedItem is not Playthrough selected)
        {
            PlaythroughMessage.Text = "Ошибка: Выберите прохождение в списке для удаления.";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var p = db.Playthroughs.Find(selected.Id);
            if (p != null)
            {
                db.Playthroughs.Remove(p);
                db.SaveChanges();
                PlaythroughMessage.Text = "Прохождение удалено!";
                _selectedPlaythroughId = null;
                LoadAllData();
            }
        }
        catch (Exception ex) { PlaythroughMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }
}