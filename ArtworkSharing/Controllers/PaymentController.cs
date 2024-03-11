using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.VNPAYS;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IVNPayTransactionService _VNPayTransactionService;

        public PaymentController(IVNPayTransactionService vNPayTransactionService, ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _VNPayTransactionService = vNPayTransactionService;
        }

        /// <summary>
        /// Get url VNPAY redirect by transactionId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUrlRedirectVNPAY(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var tran = await _transactionService.GetOne(id);
            if (tran == null!) return BadRequest();

            return Ok(_VNPayTransactionService.GetUrlFromTransaction(tran));
        }

        /// <summary>
        /// Return url and process ipnVNPAY
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ProcessIpnVNPAY()
        {
            VNPayResponseModel rs = await _VNPayTransactionService.HandleQuery(Request.QueryString + "");
            if (rs.TransactionViewModel == null)
            {
                return BadRequest(new { Message = rs.IpnResponseViewModel.Message });
            }

            return Ok(rs.TransactionViewModel);
        }

        /// <summary>
        /// Get VNPay transaction by transactionId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("VNPAYTransaction/{id}")]
        public async Task<IActionResult> GetVNPAYTransaction(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            return Ok(await _VNPayTransactionService.GetVNPayTransactionByTransactionId(id));
        }

        /// <summary>
        /// Get VNPAY Transactions
        /// </summary>
        /// <param name="vNPayFilter"></param>
        /// <returns></returns>
        [HttpGet("VNPAYTransactions")]
        public async Task<IActionResult> GetVNPAYTransactions([FromBody] VNPayFilter vNPayFilter)
         => Ok(await _VNPayTransactionService.GetVNPayTransactions(vNPayFilter));

        /// <summary>
        /// Refund transaction by transaction id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> RefundTraction([FromRoute] Guid id)
        {
            //string uId = HttpContext.Items["UserId"] + "";
            //if (string.IsNullOrEmpty(uId))
            //{
            //    return Unauthorized();
            //}
            string uId = "5612D75E-658E-47CE-C1C8-08DC40C3EE9A";
            var rs = await _VNPayTransactionService.RefundVNPay(id, Guid.Parse(uId));
            if (rs.TransactionViewModel == null)
            {
                return BadRequest(new { Message = rs.IpnResponseViewModel.Message });
            }
            return Ok(rs.TransactionViewModel);
        }
    }
}
