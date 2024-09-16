using Microsoft.Data.SqlClient;
using Npgsql;

namespace SQLApp;

public class SqlConnectionBuilder : IDisposable
{
    private static NpgsqlConnection? _npgsqlConnection;

    private static string _sql =
        "Server=localhost;Port=5432;Database=postgres; User Id = postgres; Password=password;";
    private SqlConnectionBuilder() {}

    public static NpgsqlConnection GetSqlConnection()
    {
        if (_npgsqlConnection == null) _npgsqlConnection = new NpgsqlConnection(_sql);
        return _npgsqlConnection;
    }
    public void Dispose()
    {
        if (_npgsqlConnection != null)
        {
            _npgsqlConnection.Close();
        }
    }
}