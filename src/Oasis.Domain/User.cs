using Microsoft.AspNetCore.Identity;

namespace Oasis.Domain;

public class User : IdentityUser
{
    /// <summary>
    /// 用户头像,存储 SeaweedFS 的文件key
    /// </summary>
    public string? Avatar { get; set; }
}