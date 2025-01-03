﻿using System;
using System.Collections.Generic;

namespace ApiFootballLeague.Models;

public partial class Club
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid ImageId { get; set; }

    public string Stadium { get; set; } = null!;

    public int Capacity { get; set; }

    public string? HeadCoach { get; set; }
    public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://footballleague.blob.core.windows.net/default/no-image.jpeg" : $"https://footballleague.blob.core.windows.net/clubs/{ImageId}";

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
