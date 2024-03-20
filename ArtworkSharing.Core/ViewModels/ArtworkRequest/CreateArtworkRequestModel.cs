using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Core.ViewModels.ArtworkRequest;

public class CreateArtworkRequestModel
{
    public Guid AudienceId { get; set; }
    public Guid ArtistId { get; set; }
    [Required(ErrorMessage = "Description can not be empty !")]
    public string? Description { get; set; }
    [Required]
    [Range(1, 10000,ErrorMessage = "Requested Price must be between 1 and 10,000.")]
    public float RequestedPrice { get; set; }
    [Required(ErrorMessage = "You must input the deposit amount !")]
    public float RequestedDeposit { get; set; }
    [Required]
    public DateTime RequestedDeadlineDate { get; set; }
    [JsonIgnore]
    public ICollection<Transaction>? Transactions { get; set; }

}