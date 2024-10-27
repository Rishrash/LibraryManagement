using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Core.Interfaces
{
    public interface IBookService
    {
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
    }
}