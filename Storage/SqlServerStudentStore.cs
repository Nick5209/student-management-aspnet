using CRUDWebAppInASPCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDWebAppInASPCore.Storage;

public sealed class SqlServerStudentStore(StudentDBContext context) : IStudentStore
{
    public async Task<IReadOnlyList<Student>> ListAsync() =>
        await context.Students.AsNoTracking().OrderBy(student => student.Id).ToListAsync();

    public async Task<Student?> FindAsync(int id) =>
        await context.Students.AsNoTracking().FirstOrDefaultAsync(student => student.Id == id);

    public async Task AddAsync(Student student)
    {
        await context.Students.AddAsync(student);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(int id, Student student)
    {
        var existing = await context.Students.FindAsync(id);
        if (existing is null)
        {
            return false;
        }
        existing.Name = student.Name;
        existing.Gender = student.Gender;
        existing.Age = student.Age;
        existing.Standerd = student.Standerd;
        return await SaveExistingAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await context.Students.FindAsync(id);
        if (existing is null)
        {
            return false;
        }
        context.Students.Remove(existing);
        return await SaveExistingAsync();
    }

    private async Task<bool> SaveExistingAsync()
    {
        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }
}
