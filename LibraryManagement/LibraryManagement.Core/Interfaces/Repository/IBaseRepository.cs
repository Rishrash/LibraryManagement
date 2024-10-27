using LibraryManagement.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Core.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<T> ExecuteWithinTransactionAsync<T>(Func<SqlTransaction, Task<T>> operation);
        Task<SqlConnection> CreateConnectionAsync();
    }
}
