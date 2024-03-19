using ArtworkSharing.Core.ViewModels.Transactions;
using Newtonsoft.Json;

namespace ArtworkSharing.Extensions
{
    public static class JsonExtension
    {
        /// <summary>
        /// TransactionViewModel obj to TransactionViewModel json obj
        /// </summary>
        /// <param name="tvm"></param>
        /// <returns></returns>
        public static string SerializeCategoryTransactionViewModel(this TransactionViewModel tvm)
        {
            if (tvm != null)
            {
                return JsonConvert.SerializeObject(tvm)!;
            }
            return null!;
        }
    }
}
