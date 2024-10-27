using FluentValidation;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required")
                .MaximumLength(13).WithMessage("ISBN must not exceed 13 characters");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required")
                .MaximumLength(100).WithMessage("Author must not exceed 100 characters");

            RuleFor(x => x.TotalCopies)
               .NotEmpty().WithMessage("Total copies is required")
               .GreaterThanOrEqualTo(1).WithMessage("Total copies must be at least 1");
        }
    }
}