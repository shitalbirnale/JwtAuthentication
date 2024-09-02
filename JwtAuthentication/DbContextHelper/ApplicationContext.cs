using JwtAuthentication.Model;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication.DbContextHelper
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options) { }

        public DbSet<Employee> Employees { get; set; }
    }
}
