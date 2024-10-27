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
    public class BorrowingRecordRepository : BaseRepository<BorrowingRecord>, IBorrowingRecordRepository
    {
        protected override string TableName => "BorrowingRecords";

        public BorrowingRecordRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public new async Task<BorrowingRecord> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }


        public async Task<BorrowingRecord> AddAsync(BorrowingRecord record, SqlTransaction transaction = null)
        {
            const string sql = @"
                INSERT INTO BorrowingRecords 
                (BookId, UserId, BorrowDate, DueDate, ReturnDate, CreatedAt)
                VALUES 
                (@BookId, @UserId, @BorrowDate, @DueDate, @ReturnDate, @CreatedAt);
                SELECT SCOPE_IDENTITY();";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@BookId", record.BookId),
                new SqlParameter("@UserId", record.UserId),
                new SqlParameter("@BorrowDate", record.BorrowDate),
                new SqlParameter("@DueDate", record.DueDate),
                new SqlParameter("@ReturnDate", record.ReturnDate.HasValue
                    ? (object)record.ReturnDate
                    : DBNull.Value),
                new SqlParameter("@CreatedAt", DateTime.UtcNow)
            };

            if (transaction != null)
            {
                using var command = transaction.Connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = sql;
                command.Parameters.AddRange(parameters);
                record.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            else
            {
                record.Id = Convert.ToInt32(await ExecuteScalarAsync(sql, parameters));
            }

            return record;
        }

        public async Task UpdateReturnDateAsync(int borrowingRecordId, DateTime returnDate, SqlTransaction transaction = null)
        {
            const string sql = @"
                UPDATE BorrowingRecords 
                SET ReturnDate = @ReturnDate,
                UpdatedAt = @UpdatedAt,
                UpdatedBy = @UpdatedBy
                WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
            new SqlParameter("@Id", borrowingRecordId),
            new SqlParameter("@ReturnDate", returnDate),
            new SqlParameter("@UpdatedAt", DateTime.UtcNow),
            new SqlParameter("@UpdatedBy", "System")
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

        protected override BorrowingRecord MapEntity(SqlDataReader reader)
        {
            return new BorrowingRecord
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                BorrowDate = reader.GetDateTime(reader.GetOrdinal("BorrowDate")),
                DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                ReturnDate = reader.IsDBNull(reader.GetOrdinal("ReturnDate"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }
    }
}