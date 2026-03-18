using FluentResults;
using Oasis.Application.DTOs.CabinDto;

namespace Oasis.Application.Interfaces;


public interface ICabinService
{
    /// <summary>
    /// create a new cabin asynchronously
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result> CreateCabinAsync(CreateCabinRequest request, CancellationToken cancellationToken);


    /// <summary>
    /// create multiple cabins asynchronously
    /// </summary>
    /// <param name="cabins"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result> UploadCabinsAsync(List<CreateCabinRequest> cabins, CancellationToken cancellationToken);


    /// <summary>
    /// update a cabin asynchronously
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Result> UpdateCabinAsync(long id, UpdateCabinRequest request, CancellationToken cancellationToken);



    /// <summary>
    /// delete a cabin asynchronously
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result> DeleteCabinAsync(long id, CancellationToken cancellationToken);


    /// <summary>
    /// delete all cabins asynchronously
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result> DeleteAllCabinsAsync(CancellationToken cancellationToken);




}