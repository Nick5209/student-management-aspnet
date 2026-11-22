using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CRUDWebAppInASPCore.Models
{
    public class StudentDBContext : DbContext  // DbContext is parent
    {
        public StudentDBContext(DbContextOptions options) : base(options)  // base keyword we are using for calling parent constructor and option is type of option parameter constructor
        {
            
        }
        public DbSet<Student> Students { get; set; }
    }
}
