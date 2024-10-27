using AutoMapper;
using LibraryManagement.API.DTOs;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LibraryManagement.API.Controllers
{
    public class LibraryController : BaseController
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(IMapper mapper, ILogger<LibraryController> logger, ILibraryService libraryService)
            : base(mapper, logger)
        {
            _libraryService = libraryService;
        }

        [HttpPost("borrow")]
        public async Task<ActionResult<BorrowingRecordDTO>> BorrowBook(CreateBorrowingRecordDTO borrowingDto)
        {
            try
            {
                _logger.LogInformation("Attempting to borrow book {BookId} for user {UserId}",
                    borrowingDto.BookId, borrowingDto.UserId);

                var borrowing = await _libraryService.BorrowBookAsync(
                    borrowingDto.BookId,
                    borrowingDto.UserId);

                _logger.LogInformation("Successfully created borrowing record with ID: {BorrowingId}",
                    borrowing.Id);
                return Ok(_mapper.Map<BorrowingRecordDTO>(borrowing));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found while creating borrowing record");
                return NotFound(ex.Message);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business rule violation while creating borrowing record");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating borrowing record");
                return StatusCode(500, "An error occurred while borrowing the book");
            }
        }

        [HttpPost("return/{borrowingId}")]
        public async Task<ActionResult> ReturnBook(int borrowingId)
        {
            try
            {
                _logger.LogInformation("Attempting to return book for borrowing ID: {BorrowingId}", borrowingId);
                await _libraryService.ReturnBookAsync(borrowingId);
                _logger.LogInformation("Successfully returned book for borrowing ID: {BorrowingId}", borrowingId);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Borrowing record not found: {BorrowingId}", borrowingId);
                return NotFound(ex.Message);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business rule violation while returning book");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while returning book for borrowing ID: {BorrowingId}",
                    borrowingId);
                return StatusCode(500, "An error occurred while returning the book");
            }
        }
    }
}