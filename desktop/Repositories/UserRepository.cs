using Gym_Passport.Models;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gym_Passport.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command=new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM accounts WHERE username=@username AND password=@password";
                command.Parameters.Add("@username", NpgsqlDbType.Varchar).Value = credential.UserName;
                command.Parameters.Add("@password", NpgsqlDbType.Varchar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }

        public UserModel GetByUsername(string username)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM accounts WHERE username=@username";
                command.Parameters.Add("@username", NpgsqlDbType.Varchar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Name = reader[2].ToString(),
                            Password = string.Empty,
                            Role = reader[4].ToString(),
                        };
                    }
                }
            }
            return user;
        }
    }
}
