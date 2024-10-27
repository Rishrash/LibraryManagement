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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserValidator _validator;

        public UserService(IUserRepository userRepository, UserValidator validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException(nameof(User), id);

            return user;
        }

        public async Task<User> AddUserAsync(User user)
        {
            var validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new BusinessException($"User validation failed: {errors}");
            }

            return await _userRepository.AddAsync(user);
        }

        public async Task<User> GetUserWithBorrowingHistoryAsync(int userId)
        {
            var user = await _userRepository.GetUserWithBorrowingHistoryAsync(userId);
            if (user == null)
                throw new NotFoundException(nameof(User), userId);

            return user;
        }
    }
}