using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Exceptions;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBorrowingRecordRepository _borrowingRepository;

        public LibraryService(
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IBorrowingRecordRepository borrowingRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _borrowingRepository = borrowingRepository;
        }


        public async Task<BorrowingRecord> BorrowBookAsync(int bookId, int userId)
        {
            // Get book and user
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new NotFoundException(nameof(Book), bookId);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(nameof(User), userId);

            // Check if book is available
            if (book.Status != BookStatus.Available)
                throw new BusinessException("Book is not available for borrowing");

            // Check if there are available copies
            if (book.AvailableCopies <= 0)
                throw new BusinessException("No copies available for borrowing");

            using (var connection = await _borrowingRepository.CreateConnectionAsync())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Get current UTC datetime
                    var currentDate = DateTime.UtcNow;

                    // Update book properties
                    book.AvailableCopies--;
                    if (book.AvailableCopies == 0)
                    {
                        book.Status = BookStatus.Borrowed;
                    }

                    // Create borrowing record
                    var borrowingRecord = new BorrowingRecord
                    {
                        BookId = bookId,
                        UserId = userId,
                        BorrowDate = currentDate,
                        DueDate = currentDate.AddDays(14)
                    };

                    // Update book in database
                    await _bookRepository.UpdateAsync(book, transaction);

                    // Add borrowing record
                    var addedRecord = await _borrowingRepository.AddAsync(borrowingRecord, transaction);

                    await transaction.CommitAsync();
                    return addedRecord;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new BusinessException($"Error while borrowing book: {ex.Message}");
                }
            }
        }

        public async Task<bool> ReturnBookAsync(int borrowingRecordId)
        {
            // Get borrowing record
            var record = await _borrowingRepository.GetByIdAsync(borrowingRecordId);
            if (record == null)
                throw new NotFoundException(nameof(BorrowingRecord), borrowingRecordId);

            if (record.ReturnDate.HasValue)
                throw new BusinessException("Book has already been returned");

            // Get book
            var book = await _bookRepository.GetByIdAsync(record.BookId);
            if (book == null)
                throw new NotFoundException(nameof(Book), record.BookId);

            using (var connection = await _borrowingRepository.CreateConnectionAsync())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var currentDate = DateTime.UtcNow;

                    // Update book properties
                    book.AvailableCopies++;
                    if (book.Status == BookStatus.Borrowed)
                    {
                        book.Status = BookStatus.Available;
                    }

                    // Update book in database
                    await _bookRepository.UpdateAsync(book, transaction);

                    // Update borrowing record return date
                    await _borrowingRepository.UpdateReturnDateAsync(borrowingRecordId, currentDate, transaction);

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new BusinessException($"Error while returning book: {ex.Message}");
                }
            }
        }
    }
}