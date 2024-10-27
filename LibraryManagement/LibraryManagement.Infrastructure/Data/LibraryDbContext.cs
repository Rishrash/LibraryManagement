using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Entities.Base;
using LibraryManagement.Domain.Enums;
using System.Reflection;

namespace LibraryManagement.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BorrowingRecord> BorrowingRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var currentDate = DateTime.UtcNow;

            // Seed Books
            var books = new[]
{
                new Book
                {
                    Id = 1,
                    ISBN = "9780061120084",
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Status = BookStatus.Available,
                    TotalCopies = 3,          
                    AvailableCopies = 2,      
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new Book
                {
                    Id = 2,
                    ISBN = "9780141439518",
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    Status = BookStatus.Borrowed,
                    TotalCopies = 2,          
                    AvailableCopies = 2,      
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new Book
                {
                    Id = 3,
                    ISBN = "9780743273565",
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Status = BookStatus.Available,
                    TotalCopies = 4,          
                    AvailableCopies = 4,      
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new Book
                {
                    Id = 4,
                    ISBN = "9780451524935",
                    Title = "1984",
                    Author = "George Orwell",
                    Status = BookStatus.Available,
                    TotalCopies = 3,          
                    AvailableCopies = 3,      
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new Book
                {
                    Id = 5,
                    ISBN = "9780618640157",
                    Title = "The Lord of the Rings",
                    Author = "J.R.R. Tolkien",
                    Status = BookStatus.Available,
                    TotalCopies = 2,         
                    AvailableCopies = 2,     
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                }
            };

            modelBuilder.Entity<Book>().HasData(books);

            // Seed Users
            var users = new[]
            {
                new User
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john.doe@email.com",
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new User
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@email.com",
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new User
                {
                    Id = 3,
                    Name = "Robert Johnson",
                    Email = "robert.johnson@email.com",
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new User
                {
                    Id = 4,
                    Name = "Maria Garcia",
                    Email = "maria.garcia@email.com",
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new User
                {
                    Id = 5,
                    Name = "David Wilson",
                    Email = "david.wilson@email.com",
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                }
            };

            modelBuilder.Entity<User>().HasData(users);

            // Seed Borrowing Records
            var borrowingRecords = new[]
            {
                new BorrowingRecord
                {
                    Id = 1,
                    BookId = 1,
                    UserId = 1,
                    BorrowDate = currentDate.AddDays(-10),
                    DueDate = currentDate.AddDays(4),
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new BorrowingRecord
                {
                    Id = 2,
                    BookId = 2,
                    UserId = 2,
                    BorrowDate = currentDate.AddDays(-5),
                    DueDate = currentDate.AddDays(9),
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new BorrowingRecord
                {
                    Id = 3,
                    BookId = 3,
                    UserId = 3,
                    BorrowDate = currentDate.AddDays(-20),
                    ReturnDate = currentDate.AddDays(-15),
                    DueDate = currentDate.AddDays(-6),
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                },
                new BorrowingRecord
                {
                    Id = 4,
                    BookId = 4,
                    UserId = 4,
                    BorrowDate = currentDate.AddDays(-15),
                    ReturnDate = currentDate.AddDays(-10),
                    DueDate = currentDate.AddDays(-1),
                    CreatedAt = currentDate,
                    CreatedBy = "System"
                }
            };

            modelBuilder.Entity<BorrowingRecord>().HasData(borrowingRecords);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}