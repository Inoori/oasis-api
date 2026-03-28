using FluentValidation;
using Oasis.Application.DTOs.UploadFeature;


namespace Oasis.Application.Validators;

public class UploadAvatarRequestValidator : AbstractValidator<UploadAvatarRequest>
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp"];

    public UploadAvatarRequestValidator()
    {
        RuleFor(x => x.File)
            .Must(f => AllowedExtensions.Contains(Path.GetExtension(f.FileName).ToLowerInvariant()))
            .WithMessage($"Only image files are allowed: {string.Join(", ", AllowedExtensions)}")
            .Must(f => f.Length <= 5 * 1024 * 1024) // 5 MB limit
            .WithMessage("File size must be less than or equal to 5 MB.");
    }
}