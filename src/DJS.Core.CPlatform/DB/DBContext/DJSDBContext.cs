using Microsoft.EntityFrameworkCore;

namespace DJS.Core.CPlatform.DB.DBContext
{
    public class DJSDBContext : DbContext
    {
        public DJSDBContext()
            : base()
        {
        }
        public DJSDBContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(
                "server=localhost;database=TestDb;user=test;password=123456;");
            //optionsBuilder.UseSqlServer(
            //    "server=localhost;database=TestDb;user=test;password=123456;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}
