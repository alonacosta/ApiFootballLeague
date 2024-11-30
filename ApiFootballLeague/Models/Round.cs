using System;
using System.Collections.Generic;

namespace ApiFootballLeague.Models;

public partial class Round
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public bool IsClosed { get; set; }
}
