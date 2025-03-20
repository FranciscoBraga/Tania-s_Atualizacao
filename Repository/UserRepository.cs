
using TaniasAtelie.Models;

using Npgsql;
using System.Net;


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

public async Task InsertUser(User user, IFormFile arquivo)
{

    string caminhoArquivo = await Upload(arquivo,"img");

    using (var connection = _dbConnection.GetConnection())
    {

        using (var command = connection.CreateCommand())
        {

            command.CommandText = "INSERT INTO usuario(nome,senha,foto) values (@nome,@senha,@foto)";

            var nomeParam = command.CreateParameter();
            nomeParam.ParameterName = "@nome";
            nomeParam.Value = user.Nome;
            command.Parameters.Add(nomeParam);


            var senhaParam = command.CreateParameter();
            senhaParam.ParameterName = "@senha";
            senhaParam.Value = user.Nome;
            command.Parameters.Add(senhaParam);


            var fotoParam = command.CreateParameter();
            fotoParam.ParameterName = "@foto";
            fotoParam.Value = user.Nome;
            command.Parameters.Add(fotoParam);

            command.ExecuteNonQuery();
        }
    }
}

public async Task<string> Upload(IFormFile arquivo, string pasta){

    if(arquivo == null || arquivo.Length == 0){

        return "";
    }

        string caminhoPasta = Path.Combine(_webHostEnvironment.WebRootPath,pasta);

        if(!Directory.Exists(caminhoPasta))
        Directory.CreateDirectory(caminhoPasta);

   string nomeArquivo = Guid.NewGuid(). ToString()+Path.GetExtension(arquivo.FileName);
   string caminhoCompleto = Path.Combine(caminhoPasta,nomeArquivo);

   using (var stream = new FileStream(caminhoCompleto,FileMode.Create))
   {

    await arquivo.CopyToAsync(stream);
   }     

   return $"/{pasta}/{nomeArquivo}";
} 
}