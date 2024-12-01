using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ApiFootballLeague.Models;

public partial class AspNetUser : IdentityUser
{
    //public string Id { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    //public string? UserName { get; set; }

    //public string? NormalizedUserName { get; set; }

    //public string? Email { get; set; }

    //public string? NormalizedEmail { get; set; }

    //public bool EmailConfirmed { get; set; }

    //public string? PasswordHash { get; set; }

    //public string? SecurityStamp { get; set; }

    //public string? ConcurrencyStamp { get; set; }

    //public string? PhoneNumber { get; set; }

    //public bool PhoneNumberConfirmed { get; set; }

    //public bool TwoFactorEnabled { get; set; }

    //public DateTimeOffset? LockoutEnd { get; set; }

    //public bool LockoutEnabled { get; set; }

    //public int AccessFailedCount { get; set; }

    public Guid ImageId { get; set; }

    public string ImageUrl => ImageId == Guid.Empty
           ? $"https://footballleague.blob.core.windows.net/default/no-profile.png"
           : $"https://footballleague.blob.core.windows.net/users/{ImageId}";
}
