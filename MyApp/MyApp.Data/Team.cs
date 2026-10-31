using System.Collections.Generic;

namespace MyApp.Data;

public class Team
{
    public int Id { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int MembersCount { get; set; }
    public bool IsExperienced { get; set; }

    public List<Playthrough> Playthroughs { get; set; } = new();

    public override string ToString()
    {
        return $"{TeamName} ({MembersCount} чел., опытная: {(IsExperienced ? "да" : "нет")})";
    }
}