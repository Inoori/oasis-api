using Microsoft.AspNetCore.Mvc;

namespace Oasis.Api.Controllers;

//todo 暂时不做用户相关的接口
[NonController]
public class UserController
{
    // /// <summary>
    // /// 创建用户
    // /// </summary>
    // /// <returns></returns>
    // public IActionResult CreateUser([FromBody] string userName, string password)
    // {
    //     UserManager<IdentityUser> userManager = HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
    //     var user = new IdentityUser { UserName = userName, Email = userName };
    //     var result = userManager.CreateAsync(user, password);
    //     if (result.Result.Succeeded)
    //     {
    //         return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    //     }
    //     return BadRequest(result.Result.Errors);
    // }
}