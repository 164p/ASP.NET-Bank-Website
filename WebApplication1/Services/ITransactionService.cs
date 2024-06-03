using WebApplication1.Models;
using System.Collections.Generic;

namespace WebApplication1.Services

{
    public interface ITransactionService
    {
        void Deposit(int userId, decimal amount);
        void Withdraw(int userId, decimal amount);
        void Transfer(int fromUserId, int toUserId, decimal amount);
        IEnumerable<Transaction> GetTransactionHistory(int userId);
    }
}
