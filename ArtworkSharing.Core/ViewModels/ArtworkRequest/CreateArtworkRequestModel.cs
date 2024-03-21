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
    [Range(10000, 5000000,ErrorMessage = "Requested Price must be between 10,000 and 5,000,000.")]
    public float RequestedPrice { get; set; }
    [Required(ErrorMessage = "You must input the deposit amount !")]
    public float RequestedDeposit { get; set; }
    [Required]
    public DateTime RequestedDeadlineDate { get; set; }
    [JsonIgnore]
    public ICollection<Transaction>? Transactions { get; set; }

}