using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Core.Interfaces
{
    public interface ILibraryService
    {
        Task<BorrowingRecord> BorrowBookAsync(int bookId, int userId);
        Task<bool> ReturnBookAsync(int borrowingRecordId);
    }
}