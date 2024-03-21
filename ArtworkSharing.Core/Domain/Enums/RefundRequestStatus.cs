namespace ArtworkSharing.Core.Domain.Enums
{
    public enum RefundRequestStatus
    {
        Pending,
        DenyBySystem,
        AcceptBySystem,
        DenyByArist,
        AcceptByArist

    //public static int GetStatus(string status)
    //{
    //    switch (status.ToLower())
    //    {
    //        case "pending":
    //            return RefundRequestStatus.Pending;
    //        case "DenyBySystem".ToLower():
    //            return refun;
    //        case "AcceptBySystem".ToLower():
    //            return 2;
    //        case "DenyByArist".ToLower():
    //            return 0;
    //        case "AcceptByArist".ToLower():
    //            return 0;

    //    }
    //}
    }
}
