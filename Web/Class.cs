using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

class TestConnection
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("localhost");

        using var connection = new MySqlConnection(connectionString);
        connection.Open();

        Console.WriteLine("✅ Conexión exitosa a MySQL.");
    }
}
