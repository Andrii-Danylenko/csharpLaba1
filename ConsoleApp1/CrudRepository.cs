namespace SQLApp;


// Интерфейс представляет из себя упрощенный CRUD-репозиторий.
public interface ICrudRepository
{
    bool Create(string tableName, string[] columns, string[] values);
    void Read(string tableName, params string[] columns);
    bool Update(string tableName, string[] columns, string[] values, string whereClause);
    bool Delete(string tableName, string whereClause);
}