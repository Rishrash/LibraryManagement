using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Exceptions;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Repositories;
using LibraryManagement.Application.Validators;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly BookValidator _validator;

        public BookService(IBookRepository bookRepository, BookValidator validator)
        {
            _bookRepository = bookRepository;
            _validator = validator;
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new NotFoundException(nameof(Book), id);

            return book;
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            var validationResult = await _validator.ValidateAsync(book);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new BusinessException($"Book validation failed: {errors}");
            }

            return await _bookRepository.AddAsync(book);
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _bookRepository.GetAvailableBooksAsync();
        }
    }
}