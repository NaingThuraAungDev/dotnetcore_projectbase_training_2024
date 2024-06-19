using Dapper;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using TodoApi.Models;

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

    public DbSet<Category> Category { get; set; }
    public DbSet<Book> Book { get; set; }
    public DbSet<User> User { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
        .HasKey(c => c.category_id);
        modelBuilder.Entity<Book>()
       .HasKey(c => c.book_id);
        modelBuilder.Entity<User>()
        .HasKey(c => c.user_id);
    }

}