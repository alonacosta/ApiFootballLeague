using System;
using System.Collections.Generic;

namespace ApiFootballLeague.Models;

public partial class AspNetUserRole
{
    public string UserId { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public AspNetRole? Role { get; set; }
}
