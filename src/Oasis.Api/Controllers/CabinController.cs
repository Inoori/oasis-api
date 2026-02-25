using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Api.Controllers;


[ApiController]
[Route("odata/[controller]")]
[Authorize]
public class CabinController(OasisDbContext dbContext) : ODataController
{
    [HttpGet]
    [EnableQuery]  // 启用 OData 查询选项
    public async Task<IActionResult> Get()
    {
        return Ok(dbContext.Cabins.AsQueryable());
    }

    [HttpGet("{id:int}")]
    [EnableQuery]
    public async Task<IActionResult> GetById(int id)
    {
        var result = dbContext.Cabins.Where(c => c.Id == id);
        return Ok(SingleResult.Create(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] Cabin cabin,
        CancellationToken cancellationToken)
    {
        dbContext.Cabins.Add(cabin);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Created(cabin);
    }
}