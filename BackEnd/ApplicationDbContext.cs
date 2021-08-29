using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Empleado> Empleado { get; set; }
    }
}