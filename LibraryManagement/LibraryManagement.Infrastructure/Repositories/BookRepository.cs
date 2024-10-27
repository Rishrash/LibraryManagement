using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrastructure.Repositories.Base;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        protected override string TableName => "Books";

        public BookRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<Book> AddAsync(Book book)
        {
            const string sql = @"
                INSERT INTO Books (ISBN, Title, Author, Status, TotalCopies, AvailableCopies, CreatedAt)
                VALUES (@ISBN, @Title, @Author, @Status, @TotalCopies, @AvailableCopies, @CreatedAt);
                SELECT SCOPE_IDENTITY();";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ISBN", book.ISBN),
                new SqlParameter("@Title", book.Title),
                new SqlParameter("@Author", book.Author),
                new SqlParameter("@Status", (int)book.Status),
                new SqlParameter("@TotalCopies", book.TotalCopies),
                new SqlParameter("@AvailableCopies", book.AvailableCopies.HasValue ? book.AvailableCopies.Value : book.TotalCopies),
                new SqlParameter("@CreatedAt", DateTime.UtcNow)
            };

            var id = Convert.ToInt32(await ExecuteScalarAsync(sql, parameters));
            book.Id = id;
            return book;
        }

        public new async Task<Book> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            var books = new List<Book>();

            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT *
                    FROM Books 
                    WHERE Status = @Status
                    ORDER BY Title";

                command.Parameters.AddWithValue("@Status", (int)BookStatus.Available);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        books.Add(MapEntity(reader));
                    }
                }
            }

            return books;
        }


        public async Task UpdateAsync(Book book, SqlTransaction transaction = null)
        {
            const string sql = @"
                UPDATE Books 
                SET Status = @Status,
                AvailableCopies = @AvailableCopies,
                UpdatedAt = @UpdatedAt
                WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", book.Id),
                new SqlParameter("@Status", (int)book.Status),
                new SqlParameter("@AvailableCopies", book.AvailableCopies),
                new SqlParameter("@UpdatedAt", DateTime.UtcNow)
            };

            if (transaction != null)
            {
                using var command = transaction.Connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = sql;
                command.Parameters.AddRange(parameters);
                await command.ExecuteNonQueryAsync();
            }
            else
            {
                await ExecuteNonQueryAsync(sql, parameters);
            }
        }

        protected override Book MapEntity(SqlDataReader reader)
        {
            return new Book
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Author = reader.GetString(reader.GetOrdinal("Author")),
                Status = (BookStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                TotalCopies = reader.GetInt32(reader.GetOrdinal("TotalCopies")),
                AvailableCopies = reader.GetInt32(reader.GetOrdinal("AvailableCopies")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }
    }
}