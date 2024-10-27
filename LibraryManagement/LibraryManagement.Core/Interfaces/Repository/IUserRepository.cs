using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task<User> GetUserWithBorrowingHistoryAsync(int userId);
    }
}