using CRUDWebAppInASPCore.Models;
using Microsoft.Data.Sqlite;

namespace CRUDWebAppInASPCore.Storage;

public sealed class SqliteStudentStore(string connectionString) : IStudentStore
{
    public async Task InitializeAsync()
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Students (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StudentName TEXT NOT NULL,
                StudentGender TEXT NOT NULL,
                Age INTEGER NOT NULL,
                Standerd INTEGER NOT NULL
            );
            """;
        await command.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<Student>> ListAsync()
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, StudentName, StudentGender, Age, Standerd FROM Students ORDER BY Id";
        await using var rows = await command.ExecuteReaderAsync();
        var students = new List<Student>();
        while (await rows.ReadAsync())
        {
            students.Add(ReadStudent(rows));
        }
        return students;
    }

    public async Task<Student?> FindAsync(int id)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, StudentName, StudentGender, Age, Standerd FROM Students WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);
        await using var rows = await command.ExecuteReaderAsync();
        return await rows.ReadAsync() ? ReadStudent(rows) : null;
    }

    public async Task AddAsync(Student student)
    {
        await RunAsync(
            "INSERT INTO Students (StudentName, StudentGender, Age, Standerd) VALUES ($name, $gender, $age, $standard)",
            null, student);
    }

    public async Task<bool> UpdateAsync(int id, Student student)
    {
        return await RunAsync(
            "UPDATE Students SET StudentName = $name, StudentGender = $gender, Age = $age, Standerd = $standard WHERE Id = $id",
            id, student) == 1;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await RunAsync("DELETE FROM Students WHERE Id = $id", id, null) == 1;
    }

    private async Task<int> RunAsync(string statement, int? id, Student? student)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = statement;
        if (id is int key)
        {
            command.Parameters.AddWithValue("$id", key);
        }
        if (student is not null)
        {
            command.Parameters.AddWithValue("$name", student.Name);
            command.Parameters.AddWithValue("$gender", student.Gender);
            command.Parameters.AddWithValue("$age", student.Age!.Value);
            command.Parameters.AddWithValue("$standard", student.Standerd!.Value);
        }
        return await command.ExecuteNonQueryAsync();
    }

    private static Student ReadStudent(SqliteDataReader row) => new()
    {
        Id = row.GetInt32(0),
        Name = row.GetString(1),
        Gender = row.GetString(2),
        Age = row.GetInt32(3),
        Standerd = row.GetInt32(4)
    };
}
