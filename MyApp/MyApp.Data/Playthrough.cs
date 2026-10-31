using System;

namespace MyApp.Data;

public class Playthrough
{
    public int Id { get; set; }

    public int TeamId { get; set; }
    public int QuestId { get; set; }

    public DateTime StartTime { get; set; }
    public bool IsSuccess { get; set; }

    public Team Team { get; set; } = null!;
    public Quest Quest { get; set; } = null!;

    public override string ToString()
    {
        return $"{StartTime:g}: {Team?.TeamName} проходила '{Quest?.QuestTitle}' — {(IsSuccess ? "успешно" : "провал")}";
    }
}