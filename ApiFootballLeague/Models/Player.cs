using System;
using System.Collections.Generic;

namespace ApiFootballLeague.Models;

public partial class Player
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid ImageId { get; set; }

    public int ClubId { get; set; }

    public int PositionId { get; set; }
}
