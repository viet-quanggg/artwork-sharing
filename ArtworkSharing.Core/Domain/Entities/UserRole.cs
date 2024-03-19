﻿using Microsoft.AspNetCore.Identity;

namespace ArtworkSharing.Core.Domain.Entities;

public class UserRole : IdentityUserRole<Guid>
{
    public User User { get; set; }
    public Role Role { get; set; }
}