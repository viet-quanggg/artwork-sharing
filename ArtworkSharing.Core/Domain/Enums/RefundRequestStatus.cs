namespace ArtworkSharing.Core.Domain.Enums
{
    public enum RefundRequestStatus
    {
        Pending,
        DeniedBySystem,
        AcceptedBySystem,
        DeniedByArist,
        AcceptedByArist,
        CanceledByUser,
        Payyed,
    }
}
