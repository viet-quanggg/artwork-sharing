using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.VNPAYS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService, ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _paymentService = paymentService;
        }

        /// <summary>
        /// Get url redirect by transactionId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUrlRedirect(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var tran = await _transactionService.GetOne(id);

            if (tran == null!) return BadRequest();

            return Ok(_paymentService.GetUrlFromTransaction(tran));
        }


        /// <summary>
        /// Return url and process ipn
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ProcessIpn()
        {
            VNPayViewModel rs = await _paymentService.HandleQuery(Request.QueryString + "");

            if (rs.TransactionViewModel == null)
            {
                return BadRequest(new { Message = rs.IpnResponseViewModel.Message });
            }
            return Ok(rs);
        }
    }
}
