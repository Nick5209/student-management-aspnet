using CRUDWebAppInASPCore.Models;

namespace CRUDWebAppInASPCore.Storage;

public interface IStudentStore
{
    Task<IReadOnlyList<Student>> ListAsync();
    Task<Student?> FindAsync(int id);
    Task AddAsync(Student student);
    Task<bool> UpdateAsync(int id, Student student);
    Task<bool> DeleteAsync(int id);
}
