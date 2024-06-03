using WebApplication1.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly MySqlConnection _connection;

        public UserService(IConfiguration configuration)
        {
            _connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public User Authenticate(string username, string password)
        {
            _connection.Open();
            var command = new MySqlCommand("SELECT * FROM Users WHERE Username = @username AND Password = @password", _connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32("Id"),
                        Username = reader.GetString("Username"),
                        Balance = reader.GetDecimal("Balance")
                    };
                }
            }
            _connection.Close();
            return null;
        }

        public User GetById(int id)
        {
            _connection.Open();
            var command = new MySqlCommand("SELECT * FROM Users WHERE Id = @id", _connection);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {

                    var user = new User
                    {
                        Id = reader.GetInt32("Id"),
                        Username = reader.GetString("Username"),
                        Balance = reader.GetDecimal("Balance")
                    };
                    _connection.Close();
                    return user;
                }
            }
            _connection.Close();
            return null;
        }



        public void Update(User user)
        {
            _connection.Open();
            var command = new MySqlCommand("UPDATE Users SET Balance = @balance WHERE Id = @id", _connection);
            command.Parameters.AddWithValue("@balance", user.Balance);
            command.Parameters.AddWithValue("@id", user.Id);
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
