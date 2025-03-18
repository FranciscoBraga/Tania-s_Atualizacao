using System.Threading.Tasks;
using TaniasAtelie.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Npgsql;
using TaniasAtelie.Models;

namespace TaniasAtelie.Repository;

public class UserRepository
{

private readonly DatabaseConnection _dbConnection;

private readonly IWebHostEnvironment _webHostEnvironment;

public UserRepository(DatabaseConnection databaseConnection,IWebHostEnvironment webHostEnvironment )
{

    _dbConnection = databaseConnection;
    _webHostEnvironment = webHostEnvironment;
}


public List<User> ListUser()
{

    var users =  new List<User>();

    using (var connection = _dbConnection.GetConnection())
    {

        using (var command = new NpgsqlCommand("Select id_usuario,nome,email,login,senha,foto from usuario", (NpgsqlConnection)connection))
        {

            using(var reader =  command.ExecuteReader())
            {

                while (reader.Read())
                {

                    users.Add(new User
                    {

                       Id = reader.GetInt32(0),
                       Nome = reader.GetString(1),
                       Email = reader.GetString(2),
                       Login = reader.GetString(3),
                       Senha = reader.GetString(4),
                       Foto = reader.IsDBNull(4)? null: reader.GetString(5),

                    } );
                }
            }

        }
}

return users;

}
}