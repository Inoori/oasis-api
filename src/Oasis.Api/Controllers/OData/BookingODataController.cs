using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Api.Controllers.OData;


[ApiController]
[Route("odata/bookings")]
public class BookingODataController(OasisDbContext dbContext) : ODataController
{
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        return Ok(dbContext.Bookings.AsQueryable());
    }
}
