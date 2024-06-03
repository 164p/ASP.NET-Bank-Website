using WebApplication1.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using WebApplication1.Services;
using System.Data;

namespace YourProject.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly MySqlConnection _connection;
        private readonly IUserService _userService;

        public TransactionService(IConfiguration configuration, IUserService userService)
        {
            _connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            _userService = userService;
        }

        public void Deposit(int userId, decimal amount)
        {
            var user = _userService.GetById(userId);
            user.Balance += amount;
            _userService.Update(user);

            _connection.Open();
            var command = new MySqlCommand("INSERT INTO Transactions (UserId, ActionType, Amount, Remain) VALUES (@userId, 'Deposit', @amount, @remain)", _connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@remain", user.Balance);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public void Withdraw(int userId, decimal amount)
        {
            var user = _userService.GetById(userId);
            if (user.Balance >= amount)
            {
                user.Balance -= amount;
                _userService.Update(user);

                _connection.Open();
                var command = new MySqlCommand("INSERT INTO Transactions (UserId, ActionType, Amount, Remain) VALUES (@userId, 'Withdraw', @amount, @remain)", _connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@remain", user.Balance);
                command.ExecuteNonQuery();
                _connection.Close();
            }
            else
            {
                throw new Exception("Insufficient funds");
            }
        }

        public void Transfer(int fromUserId, int toUserId, decimal amount)
        {
            var fromUser = _userService.GetById(fromUserId);
            var toUser = _userService.GetById(toUserId);

            if (fromUser == null)
            {
                throw new Exception("From user does not exist.");
            }

            if (toUser == null)
            {
                throw new Exception("To user does not exist.");
            }

            if (fromUserId != toUserId)
            {
                if (fromUser.Balance >= amount)
                {
                    fromUser.Balance -= amount;
                    toUser.Balance += amount;
                    _userService.Update(fromUser);
                    _userService.Update(toUser);

                    _connection.Open();
                    var transfer = new MySqlCommand("INSERT INTO Transactions (UserId, ActionType, FromUserId, Amount, Remain) VALUES (@userId, 'Transfer', @fromUserId, @amount, @remain)", _connection);
                    transfer.Parameters.AddWithValue("@userId", toUserId);
                    transfer.Parameters.AddWithValue("@fromUserId", fromUserId);
                    transfer.Parameters.AddWithValue("@amount", amount);
                    transfer.Parameters.AddWithValue("@remain", fromUser.Balance);
                    transfer.ExecuteNonQuery();
                    var received = new MySqlCommand("INSERT INTO Transactions (UserId, ActionType, FromUserId, Amount, Remain) VALUES (@userId, 'Received', @fromUserId, @amount, @remain)", _connection);
                    received.Parameters.AddWithValue("@userId", toUserId);
                    received.Parameters.AddWithValue("@fromUserId", fromUserId);
                    received.Parameters.AddWithValue("@amount", amount);
                    received.Parameters.AddWithValue("@remain", toUser.Balance);
                    received.ExecuteNonQuery();
                    _connection.Close();
                }
                else
                {
                    throw new Exception("Insufficient funds");
                }
            }
            else
            {
                throw new Exception("Can transfer to this account");
            }
        }

        public IEnumerable<Transaction> GetTransactionHistory(int userId)
        {
            var transactions = new List<Transaction>();

            _connection.Open();
            var command = new MySqlCommand("SELECT * FROM Transactions WHERE UserId = @userId OR FromUserId = @userId", _connection);
            command.Parameters.AddWithValue("@userId", userId);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    transactions.Add(new Transaction
                    {
                        Id = reader.GetInt32("Id"),
                        UserId = reader.GetInt32("UserId"),
                        ActionType = reader.GetString("ActionType"),
                        FromUserId = reader.IsDBNull(reader.GetOrdinal("FromUserId")) ? (int?)null : reader.GetInt32("FromUserId"),
                        Amount = reader.GetDecimal("Amount"),
                        Date = reader.GetDateTime("Date"),
                        Remain = reader.GetDecimal("Remain"),
                    });
                }
            }
            _connection.Close();

            return transactions;
        }
    }
}
