using FluentValidation;
using Oasis.Application.DTOs;
using Oasis.Application.DTOs.CabinDto;

namespace Oasis.Application.Validators;

public class CreateCabinRequestValidator : AbstractValidator<CreateCabinRequest>
{
    public CreateCabinRequestValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("cabin name cannot be empty")
            .MaximumLength(100).WithMessage("cabin name cannot exceed 100 characters");
    }
}