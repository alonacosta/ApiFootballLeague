using System;
using System.Collections.Generic;

namespace ApiFootballLeague.Models;

public partial class StaffMember
{
    public int Id { get; set; }

    public int ClubId { get; set; }

    public int FunctionId { get; set; }

    public string? UserId { get; set; }
}
