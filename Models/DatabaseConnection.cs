using Npgsql;
using Microsoft.Extensions.Configuration;

public class DatabaseConnection
{

    private readonly string? _connectionString;

    public DatabaseConnection(IConfiguration configuration)
    {

        _connectionString = configuration.GetConnectionString("DefaultConnection");
       
    }

    public NpgsqlConnection GetConnection()
    {
        Console.WriteLine("connection");
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        Console.WriteLine(connection +"connection");
        return connection;
        
    }
}
