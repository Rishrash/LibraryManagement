using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using LibraryManagement.Domain.Entities.Base;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Infrastructure.Repositories.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly string _connectionString;

        protected BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<SqlConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<SqlTransaction> BeginTransactionAsync()
        {
            var connection = await CreateConnectionAsync();
            return connection.BeginTransaction();
        }

        public async Task<T> ExecuteWithinTransactionAsync<T>(Func<SqlTransaction, Task<T>> operation)
        {
            using var connection = await CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var result = await operation(transaction);
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        protected abstract T MapEntity(SqlDataReader reader);
        protected abstract string TableName { get; }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {TableName} WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapEntity(reader);
                    }
                }
            }
            return null;
        }

        protected async Task<int> ExecuteNonQueryAsync(string sql, SqlParameter[] parameters = null)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = sql;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                return await command.ExecuteNonQueryAsync();
            }
        }

        protected async Task<object> ExecuteScalarAsync(string sql, SqlParameter[] parameters = null)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = sql;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                return await command.ExecuteScalarAsync();
            }
        }
    }
}