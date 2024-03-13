using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using FluentValidation;

namespace ArtworkSharing.Service.Validators;

public class UserToLoginDTOValidator : AbstractValidator<UserToLoginDto>
{
    public UserToLoginDTOValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}