using System.ComponentModel.DataAnnotations;

namespace ArtworkSharing.Core.ViewModels.VNPAYS
{
    public class IpnModel
    {
        [Required]
        public string vnp_TmnCode { get; set; } = null!;

        [Required]
        public int vnp_Amount { get; set; }

        [Required]
        public string vnp_BankCode { get; set; } = null!;

        public string? vnp_BankTranNo { get; set; }

        public string? vnp_CardType { get; set; }

        public string? vnp_PayDate { get; set; }

        [Required]
        public string vnp_OrderInfo { get; set; } = null!;

        [Required]
        public string vnp_TransactionNo { get; set; } = null!;

        [Required]
        public string vnp_ResponseCode { get; set; } = null!;

        [Required]
        public string vnp_TransactionStatus { get; set; } = null!;

        [Required]
        public string vnp_TxnRef { get; set; } = null!;

        public string? vnp_SecureHashType { get; set; }

        [Required]
        public string vnp_SecureHash { get; set; } = null!;
    }
}
