using CapstoneProject.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject.DBContext
{
    public class CapStoneDbContext : DbContext
    {
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDoEntry> ToDoEntries { get; set; }

        public CapStoneDbContext(DbContextOptions<CapStoneDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One to Many relationship
            modelBuilder.Entity<ToDoEntry>()
                .HasOne(p => p.ToDoList)
                .WithMany(b => b.ToDoEntries)
                .HasForeignKey(x => x.ToDoListId);

            modelBuilder.Entity<ToDoEntry>().ToTable("ToDoEntry");
            modelBuilder.Entity<ToDoList>().ToTable("ToDoList");
        }
    }
}
