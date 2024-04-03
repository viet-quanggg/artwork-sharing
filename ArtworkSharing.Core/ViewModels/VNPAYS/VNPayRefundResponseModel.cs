﻿namespace ArtworkSharing.Core.ViewModels.VNPAYS;

public class VNPayRefundResponseModel
{
    public string vnp_ResponseId { get; set; }
    public string vnp_Command { get; set; }
    public string vnp_ResponseCode { get; set; }
    public string vnp_Message { get; set; }
    public string vnp_TmnCode { get; set; }
    public string vnp_TxnRef { get; set; }
    public double vnp_Amount { get; set; }
    public string vnp_BankCode { get; set; }
    public string vnp_PayDate { get; set; }
    public string vnp_TransactionNo { get; set; }
    public string vnp_TransactionType { get; set; }
    public string vnp_TransactionStatus { get; set; }
}