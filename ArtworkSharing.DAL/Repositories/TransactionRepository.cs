using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.DAL.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly DbContext _dbContext;

        public TransactionRepository(DbContext dbContext) : base(dbContext)
        {
            _dbContext=dbContext;
        }

        public void UpdateTransaction(Transaction transaction)
        {
            _dbContext.Update<Transaction>(transaction);
        }
    }
}
