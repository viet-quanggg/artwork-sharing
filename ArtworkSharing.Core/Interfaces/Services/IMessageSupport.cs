using ArtworkSharing.Core.Models;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IMessageSupport
    {
        Task RaiseEventPayment(MessageRaw  messageRaw, CancellationToken cancellationToken = default);
    }
}
