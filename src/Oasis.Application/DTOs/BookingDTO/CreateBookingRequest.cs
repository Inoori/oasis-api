using System.Text.Json.Serialization;
using Mapster;
using Oasis.Domain;

namespace Oasis.Application.DTOs.BookingDTO;

public sealed class CreateBookingRequest : IMapFrom<Booking>
{

    /// <summary>
    /// 预订的开始日期
    /// </summary>
    public DateTimeOffset? StartDate { get; set; }

    /// <summary>
    /// 预订的结束日期
    /// </summary>
    public DateTimeOffset? EndDate { get; set; }

    /// <summary>
    /// 预订的夜晚数
    /// </summary>
    public short? NumNights { get; set; }

    /// <summary>
    /// 预订的客人数
    /// </summary>
    public short? NumGuests { get; set; }

    /// <summary>
    /// 小屋价格
    /// </summary>
    public float? CabinPrice { get; set; }

    /// <summary>
    /// 额外服务价格
    /// </summary>
    public float? ExtrasPrice { get; set; }

    /// <summary>
    /// 总价格
    /// </summary>
    public float? TotalPrice { get; set; }

    /// <summary>
    /// 预订状态
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BookingStatus Status { get; set; }

    /// <summary>
    /// 是否包含早餐
    /// </summary>
    public bool? HasBreakfast { get; set; }

    /// <summary>
    /// 是否已支付
    /// </summary>
    public bool? IsPaid { get; set; }

    /// <summary>
    /// 备注或特殊要求
    /// </summary>
    public string? Observations { get; set; }

    /// <summary>
    /// 关联的小屋 Id
    /// </summary>
    public long? CabinId { get; set; }

    /// <summary>
    /// 关联的客人 Id
    /// </summary>
    public long? GuestId { get; set; }
}