namespace ArtworkSharing.Core.Domain.Enums
{
    public static class RoutingKey
    {
        public const string PaymentRaise = "payment-raise";
        public const string PaidRaise = "paid-raise";
    }

    public static class Exchange
    {
        public const string PaymentRaise = "PaymentRaise";
        public const string PaidRaise = "PaidRaise";
    }

    public static class Queue
    {
        public const string PaymentRaiseQueue = "PaymentRaiseQueue";
        public const string PaidRaiseQueue = "PaidRaiseQueue";
    }
}
