using LMSCapital.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LMSCapital.Services
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Models.Db.Book> Books { get; set; }
        public DbSet<Models.Db.User> Users { get; set; }
        public DbSet<Models.Db.IssuedBook> IssuedBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Foreign Key for User
            modelBuilder.Entity<IssuedBook>()
                .HasOne(i => i.User)  
                .WithMany(u => u.IssuedBooks) 
                .HasForeignKey(i => i.UserId)  
                .OnDelete(DeleteBehavior.Restrict);

            // Foreign Key for Book
            modelBuilder.Entity<IssuedBook>()
                .HasOne(i => i.Book)
                .WithMany(b => b.IssuedBooks)
                .HasForeignKey(i => i.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }



}
