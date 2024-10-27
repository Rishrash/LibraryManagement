using System.Data.SqlClient;
using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Core.Interfaces
{
    public interface IBorrowingRecordRepository : IBaseRepository<BorrowingRecord>
    {
        Task<BorrowingRecord> GetByIdAsync(int id);
        Task<BorrowingRecord> AddAsync(BorrowingRecord record, SqlTransaction transaction = null);
        Task UpdateReturnDateAsync(int borrowingRecordId, DateTime returnDate, SqlTransaction transaction = null);
    }
}