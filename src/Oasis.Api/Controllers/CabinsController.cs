using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class CabinsController(OasisDbContext dbContext) : ODataController
{
    [HttpGet]
    [Route("odata/cabins")]  // 显式指定 OData 路由
    [EnableQuery]  // 启用 OData 查询选项
    public async Task<IActionResult> Get()
    {
        return Ok(dbContext.Cabins.AsQueryable());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = dbContext.Cabins.Where(c => c.Id == id);
        return Ok(SingleResult.Create(result));
    }

    [HttpPost]
    [Route("~/odata/cabins")]
    public async Task<IActionResult> Create(
        [FromBody] Cabin cabin,
        CancellationToken cancellationToken)
    {
        //todo:validate data

        dbContext.Cabins.Add(cabin);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Created(cabin);
    }

    [HttpPost]
    [Route("~/odata/cabins/batch")]
    public async Task<IActionResult> CreateCabins(
        [FromBody] List<Cabin> cabins,
        CancellationToken cancellationToken)
    {
        dbContext.Cabins.AddRange(cabins);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok();
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] Cabin cabin, CancellationToken cancellationToken)
    {
        var existingCabin = await dbContext.Cabins.AsTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (existingCabin == null)
        {
            return NotFound();
        }

        // 更新属性 
        existingCabin.Name = cabin.Name;
        existingCabin.Description = cabin.Description;
        existingCabin.Discount = cabin.Discount;
        existingCabin.MaxCapacity = cabin.MaxCapacity;
        existingCabin.RegularPrice = cabin.RegularPrice;

        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok();

    }


    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var cabin = await dbContext.Cabins.FindAsync([id], cancellationToken);

        if (cabin == null)
        {
            return NotFound();
        }

        dbContext.Cabins.Remove(cabin);
        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteAll(CancellationToken cancellationToken)
    {
        await dbContext.Cabins.ExecuteDeleteAsync(cancellationToken);
        return NoContent();
    }
}