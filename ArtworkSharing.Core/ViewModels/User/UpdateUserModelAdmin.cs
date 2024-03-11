using System.Text.Json.Serialization;
using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.ViewModels.User;

public class UpdateUserModelAdmin
{
    public string Name { get; set; } = null!;      
    public string? PhotoUrl { get; set; }        
    public string? Gender { get; set; }
    public bool Status { get; set; }
    


}