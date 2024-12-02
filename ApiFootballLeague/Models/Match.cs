using System;
using System.Collections.Generic;

namespace ApiFootballLeague.Models;

public partial class Match
{
    public int Id { get; set; }

    public int RoundId { get; set; }

    public string? HomeTeam { get; set; }

    public string? AwayTeam { get; set; }

    public int HomeScore { get; set; }

    public int AwayScore { get; set; }

    public bool IsClosed { get; set; }

    public DateTime StartDate { get; set; }

    public bool IsFinished { get; set; }

    public virtual Round Round { get; set; } = null!;
}
