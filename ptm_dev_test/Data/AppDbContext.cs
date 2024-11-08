using Microsoft.EntityFrameworkCore;
using ptm_dev_test.Models;

namespace ptm_dev_test.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ExamesModel> Exames { get; set; }
    }
}
