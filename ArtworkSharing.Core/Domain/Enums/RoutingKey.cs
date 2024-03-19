namespace ArtworkSharing.Core.Domain.Enums
{
    public static class RoutingKey
    {
        public const string PaymentRaise = "payment-raise";
        public const string PaidRaise = "paid-raise";
        public const string RefundPaymentRaise = "refund-payment-raise";
        public const string RefundPaidRaise = "refund-paid-raise";
        public const string PaypalPaymentRaise = "paypal-payment-raise";
        public const string PaypalPaidRaise = "paypal-paid-raise";
        public const string RefundPaypalPaymentRaise = "refund-paypal-payment-raise";
        public const string RefundPaypalPaidRaise = "refund-paypal-paid-raise";
    }
    public static class Exchange
    {
        public const string PaymentRaise = "PaymentRaise";
        public const string PaidRaise = "PaidRaise";
        public const string RefundPaymentRaise = "RefundPaymentRaise";
        public const string RefundPaidRaise = "RefundPaidRaise";
        public const string PaypalPaymentRaise = "PaypalPaymentRaise";
        public const string PaypalPaidRaise = "PaypalPaidRaise";
        public const string RefundPaypalPaymentRaise = "RefundPaypalPaymentRaise";
        public const string RefundPaypalPaidRaise = "RefundPaypalPaidRaise";
    }
    public static class Queue
    {
        public const string PaymentRaiseQueue = "PaymentRaiseQueue";
        public const string PaidRaiseQueue = "PaidRaiseQueue";
        public const string RefundPaymentRaiseQueue = "RefundPaymentRaiseQueue";
        public const string RefundPaidRaiseQueue = "RefundPaidRaiseQueue";
        public const string PaypalPaymentRaiseQueue = "PaypalPaymentRaiseQueue";
        public const string PaypalPaidRaiseQueue = "PaypalPaidRaiseQueue";
        public const string RefundPaypalPaymentRaiseQueue = "RefundPaypalPaymentRaiseQueue";
        public const string RefundPaypalPaidRaiseQueue = "RefundPaypalPaidRaiseQueue";
    }
}
