using System.Data.SqlClient;
using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Core.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<Book> GetByIdAsync(int id);
        Task<Book> AddAsync(Book book);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task UpdateAsync(Book book, SqlTransaction transaction = null);
    }
}