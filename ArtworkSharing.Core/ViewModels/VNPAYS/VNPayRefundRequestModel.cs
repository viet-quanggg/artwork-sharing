namespace ArtworkSharing.Core.ViewModels.VNPAYS;

public class VNPayRefundRequestModel
{
    public string vnp_RequestId { get; set; }
    public string vnp_Version { get; set; }
    public string vnp_Command { get; set; }
    public string vnp_TmnCode { get; set; }
    public string vnp_TransactionType { get; set; }
    public long vnp_TxnRef { get; set; }
    public string vnp_Amount { get; set; }
    public string vnp_TransactionNo { get; set; }
    public string vnp_TransactionDate { get; set; }
    public string vnp_CreateBy { get; set; }
    public string vnp_CreateDate { get; set; }
    public string vnp_IpAddr { get; set; }
    public string vnp_OrderInfo { get; set; }
    public string vnp_SecureHash { get; set; }
}