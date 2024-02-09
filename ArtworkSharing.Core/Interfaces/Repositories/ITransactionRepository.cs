using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ArtworkSharing.Core.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
    }
}
