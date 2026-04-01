using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Api.Controllers.OData;


[ApiController]
[Route("odata/guests")]
[Authorize]
public class GuestODataController(OasisDbContext dbContext) : ODataController
{
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        return Ok(dbContext.Guests.AsQueryable());
    }
}
