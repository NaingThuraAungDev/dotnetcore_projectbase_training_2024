using Dapper;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

public class AppDB : DbContext
{

    readonly string connectionString = "";
    public AppDB(DbContextOptions<AppDB> options) : base(options)
    {
        var appsettingbuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var Configuration = appsettingbuilder.Build();
        connectionString = Configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<T?> GetAsync<T>(string command, object parms)
    {
        T? result;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            result = (await connection.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();
        }
        return result;
    }

    public async Task<List<T>> GetAll<T>(string command, object parms)
    {
        List<T> result = new List<T>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            result = (await connection.QueryAsync<T>(command, parms)).ToList();
        }
        return result;
    }

    public async Task<int> EditData(string command, object parms)
    {
        int result;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            result = await connection.ExecuteAsync(command, parms);
        }
        return result;
    }
}