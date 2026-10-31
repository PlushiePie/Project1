using System.Collections.Generic;

namespace MyApp.Data;

public class Quest
{
    public int Id { get; set; }
    public string QuestTitle { get; set; } = string.Empty;
    public int DifficultyLevel { get; set; }
    public int DurationMinutes { get; set; }
    public int ReagentsCount { get; set; }

    public List<Playthrough> Playthroughs { get; set; } = new();

    public override string ToString()
    {
        return $"{QuestTitle} (сложность: {DifficultyLevel}, {DurationMinutes} мин., реактивов: {ReagentsCount})";
    }
}