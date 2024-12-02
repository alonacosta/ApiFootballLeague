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
    public string ImageUrl => ImageId == Guid.Empty
           ? "https://footballleague.blob.core.windows.net/default/no-profile.png"
           : $"https://footballleague.blob.core.windows.net/players/{ImageId}";

    public virtual Club Club { get; set; } = null!;
    public virtual Position Position { get; set; } = null!;
}
