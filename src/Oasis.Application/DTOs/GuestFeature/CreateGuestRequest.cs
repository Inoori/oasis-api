

using Mapster;
using Oasis.Domain;

namespace Oasis.Application.DTOs.GuestFeature;

public sealed class CreateGuestRequest : IMapFrom<Guest>
{
    /// <summary>
    /// 客人全名
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 国籍
    /// </summary>
    public string? Nationality { get; set; }

    /// <summary>
    /// 国家旗帜
    /// </summary>
    public string? CountryFlag { get; set; }

    /// <summary>
    /// 国家身份证号码
    /// </summary>
    public string? NationalID { get; set; }
}