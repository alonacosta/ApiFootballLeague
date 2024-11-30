using System;
using System.Collections.Generic;

namespace ApiFootballLeague.Models;

public partial class Incident
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public string OccurenceName { get; set; } = null!;

    public DateTime EventTime { get; set; }

    public int PlayerId { get; set; }
}
