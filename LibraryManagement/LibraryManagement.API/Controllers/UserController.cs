using AutoMapper;
using LibraryManagement.API.DTOs;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Exceptions;
using LibraryManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LibraryManagement.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IMapper mapper, ILogger<UserController> logger, IUserService userService)
            : base(mapper, logger)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve user with ID: {UserId}", id);
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID: {UserId} was not found", id);
                    return NotFound($"User with ID {id} not found");
                }

                _logger.LogInformation("Successfully retrieved user with ID: {UserId}", id);
                return Ok(_mapper.Map<UserDTO>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user with ID: {UserId}", id);
                return StatusCode(500, "An error occurred while retrieving the user");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO userDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create new user with email: {Email}", userDto.Email);

                var user = _mapper.Map<User>(userDto);
                var result = await _userService.AddUserAsync(user);

                _logger.LogInformation("Successfully created user with ID: {UserId}", result.Id);
                return CreatedAtAction(nameof(GetUser), new { id = result.Id }, _mapper.Map<UserDTO>(result));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating user: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                return StatusCode(500, "An error occurred while creating the user");
            }
        }

        [HttpGet("{id}/borrowing-history")]
        public async Task<ActionResult<UserDTO>> GetUserWithBorrowingHistory(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve borrowing history for user ID: {UserId}", id);
                var user = await _userService.GetUserWithBorrowingHistoryAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID: {UserId} was not found", id);
                    return NotFound($"User with ID {id} not found");
                }

                _logger.LogInformation("Successfully retrieved borrowing history for user ID: {UserId}", id);
                return Ok(_mapper.Map<UserDTO>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving borrowing history for user ID: {UserId}", id);
                return StatusCode(500, "An error occurred while retrieving the user's borrowing history");
            }
        }
    }
}