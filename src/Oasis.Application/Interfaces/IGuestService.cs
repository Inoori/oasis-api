using FluentResults;
using Oasis.Application.DTOs.GuestDTO;

namespace Oasis.Application.Interfaces;

public interface IGuestService
{
    /// <summary>
    /// Creates multiple guests asynchronously.
    /// </summary>
    /// <param name="guests">The list of guests to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> UploadGuestsAsync(List<CreateGuestRequest> guests, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes all guests asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> DeleteAllGuestsAsync(CancellationToken cancellationToken);
}