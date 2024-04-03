namespace ArtworkSharing.Core.Domain.Enums
{
    public static class RoutingKey
    {
        public const string PaidRaise = "paid-raise";
        public const string PaypalPaidRaise = "paypal-paid-raise";
        public const string RefundPaidRaise = "refund-paid-raise";
        public const string RefundPaypalRaise = "refund-paypal-raise";
    }
    public static class Exchange
    {
        public const string PaidRaise = "PaidRaise";
        public const string PaypalPaidRaise = "PaypalPaidRaise";
        public const string RefundPaidRaise = "RefundPaidRaise";
        public const string RefundPaypaldRaise = "RefundPaypaldRaise";

    }
    public static class Queue
    {
        public const string PaidRaiseQueue = "PaidRaiseQueue";
        public const string PaypalPaidRaiseQueue = "PaypalPaidRaiseQueue";
        public const string RefundPaidRaiseQueue = "RefundPaidRaiseQueue";
        public const string RefundPaypalRaiseQueue = "RefundPaypalRaiseQueue";

    }
}
