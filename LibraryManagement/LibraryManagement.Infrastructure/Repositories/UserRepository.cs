using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Repositories.Base;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        protected override string TableName => "Users";

        public UserRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public new async Task<User> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<User> AddAsync(User user)
        {
            const string sql = @"
                INSERT INTO Users (Name, Email, CreatedAt)
                VALUES (@Name, @Email, @CreatedAt);
                SELECT SCOPE_IDENTITY();";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", user.Name),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@CreatedAt", DateTime.UtcNow)
            };

            var id = Convert.ToInt32(await ExecuteScalarAsync(sql, parameters));
            user.Id = id;
            return user;
        }

        public async Task<User> GetUserWithBorrowingHistoryAsync(int userId)
        {
            User user = null;

            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT u.*, 
                           br.Id as BorrowingId, 
                           br.BookId,
                           br.BorrowDate,
                           br.ReturnDate,
                           br.DueDate,
                           br.CreatedAt as BorrowingCreatedAt,
                           b.ISBN,
                           b.Title,
                           b.Author,
                           b.Status
                    FROM Users u
                    LEFT JOIN BorrowingRecords br ON u.Id = br.UserId
                    LEFT JOIN Books b ON br.BookId = b.Id
                    WHERE u.Id = @UserId
                    ORDER BY br.BorrowDate DESC";

                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (user == null)
                        {
                            user = MapEntity(reader);
                            user.BorrowingHistory = new List<BorrowingRecord>();
                        }

                        // Check if there are any borrowing records
                        if (!reader.IsDBNull(reader.GetOrdinal("BorrowingId")))
                        {
                            var borrowingRecord = new BorrowingRecord
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BorrowingId")),
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                UserId = userId,
                                BorrowDate = reader.GetDateTime(reader.GetOrdinal("BorrowDate")),
                                ReturnDate = reader.IsDBNull(reader.GetOrdinal("ReturnDate"))
                                    ? null
                                    : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                                DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("BorrowingCreatedAt")),
                                Book = new Book
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BookId")),
                                    ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Author = reader.GetString(reader.GetOrdinal("Author")),
                                    Status = (BookStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                                }
                            };

                            user.BorrowingHistory.Add(borrowingRecord);
                        }
                    }
                }
            }

            return user;
        }

        protected override User MapEntity(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }
    }
}