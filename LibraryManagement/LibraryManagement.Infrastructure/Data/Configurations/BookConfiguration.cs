using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Infrastructure.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ISBN)
                .IsRequired()
                .HasMaxLength(13);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Author)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.HasIndex(x => x.ISBN)
                .IsUnique();

            builder.HasMany(x => x.BorrowingHistory)
                .WithOne(x => x.Book)
                .HasForeignKey(x => x.BookId);
        }
    }
}