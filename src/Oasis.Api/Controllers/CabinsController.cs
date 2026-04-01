using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Oasis.Application.DTOs.CabinFeature;
using Oasis.Application.Interfaces;

namespace Oasis.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CabinsController(ICabinService cabinService) : ODataController
{

    [HttpPost]
    public async Task<IActionResult> CreateCabin([FromBody] CreateCabinRequest request, CancellationToken cancellationToken)
    {
        var result = await cabinService.CreateCabinAsync(request, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Errors);
    }


    [HttpPost("upload")]
    public async Task<IActionResult> UploadCabins(
        [FromBody] List<CreateCabinRequest> cabins,
        CancellationToken cancellationToken)
    {
        var result = await cabinService.UploadCabinsAsync(cabins, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Errors);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCabinRequest request, CancellationToken cancellationToken)
    {
        var result = await cabinService.UpdateCabinAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Errors);
    }


    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var result = await cabinService.DeleteCabinAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteAll(CancellationToken cancellationToken)
    {
        var result = await cabinService.DeleteAllCabinsAsync(cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
    }
}