﻿namespace ArtworkSharing.Core.ViewModels.Users;

public class UpdateUserModel
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? PhotoUrl { get; set; }
    public string? Gender { get; set; }
}