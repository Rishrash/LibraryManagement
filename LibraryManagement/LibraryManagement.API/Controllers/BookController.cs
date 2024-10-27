using Microsoft.AspNetCore.Mvc;
using LibraryManagement.API.DTOs;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Exceptions;
using LibraryManagement.Domain.Entities;
using AutoMapper;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LibraryManagement.API.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;

        public BookController(IMapper mapper, ILogger<BookController> logger, IBookService bookService)
            : base(mapper, logger)
        {
            _bookService = bookService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve book with ID: {BookId}", id);
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    _logger.LogWarning("Book with ID: {BookId} was not found", id);
                    return NotFound($"Book with ID {id} not found");
                }

                _logger.LogInformation("Successfully retrieved book with ID: {BookId}", id);
                return Ok(_mapper.Map<BookDTO>(book));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving book with ID: {BookId}", id);
                return StatusCode(500, "An error occurred while retrieving the book");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook(CreateBookDTO bookDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create new book with ISBN: {ISBN}", bookDto.ISBN);

                var book = _mapper.Map<Book>(bookDto);
                var result = await _bookService.AddBookAsync(book);

                _logger.LogInformation("Successfully created book with ID: {BookId}", result.Id);
                return CreatedAtAction(nameof(GetBook), new { id = result.Id }, _mapper.Map<BookDTO>(result));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating book: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating book");
                return StatusCode(500, "An error occurred while creating the book");
            }
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAvailableBooks()
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve available books");
                var books = await _bookService.GetAvailableBooksAsync();
                _logger.LogInformation("Successfully retrieved {Count} available books", books?.Count() ?? 0);
                return Ok(_mapper.Map<IEnumerable<BookDTO>>(books));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving available books");
                return StatusCode(500, "An error occurred while retrieving available books");
            }
        }
    }
}