using Npgsql;

namespace SQLApp;

public class Repository : ICrudRepository
{
    private static NpgsqlConnection _sqlConnection = SqlConnectionBuilder.GetSqlConnection();

    public bool Create(string tableName, string[] columns, string[] values)
    {
        if (columns.Length != values.Length)
        {
            throw new ArgumentException("Длина параметров для вставки должна соответствовать количеству параметров вставки!");
        }
        string columnsPart = string.Join(", ", columns);
        string valuesPart = string.Join(", ", values.Select(v => $"'{v}'"));
        string query = $"INSERT INTO {tableName} ({columnsPart}) VALUES ({valuesPart})";
        Console.WriteLine(query);
        using (var command = new NpgsqlCommand(query, _sqlConnection))
        {
            _sqlConnection.Open();
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }

    public void Read(string tableName, params string[] columns)
    {
        string columnsPart = columns.Length > 0 ? string.Join(", ", columns) : "*";
        string query = $"SELECT {columnsPart} FROM {tableName}";
        Console.WriteLine(query);
        using (var command = new NpgsqlCommand(query, _sqlConnection))
        {
            _sqlConnection.Open();
            try
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetValue(i)}\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }

    public bool Update(string tableName, string[] columns, string[] values, string whereClause)
    {
        if (columns.Length != values.Length)
        {
            throw new ArgumentException("Длина параметров для обновления должна соответствовать количеству параметров обновления!");
        }
        string setClause = string.Join(", ", columns.Zip(values, (column, value) => $"{column} = '{value}'"));
        string query = $"UPDATE {tableName} SET {setClause} WHERE {whereClause}";
        Console.WriteLine(query);
        using (var command = new NpgsqlCommand(query, _sqlConnection))
        {
            _sqlConnection.Open();
            try
            {
                int affectedRows = command.ExecuteNonQuery();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }

    
    // Postgres не умеет в каскадное удаление. Я не стал его реализовывать через рекурсию, потому что мне лень :)
    public bool Delete(string tableName, string whereClause)
    {
        string query = $"DELETE FROM {tableName} WHERE {whereClause}";
        Console.WriteLine(query);
        using (var command = new NpgsqlCommand(query, _sqlConnection))
        {
            _sqlConnection.Open();
            try
            {
                int affectedRows = command.ExecuteNonQuery();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}
