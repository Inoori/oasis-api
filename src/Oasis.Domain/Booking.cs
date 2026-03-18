using System.Text.Json.Serialization;
using FluentResults;

namespace Oasis.Domain;

public class Booking
{
    public long Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

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
    /// 预订状态（如：已确认、已取消等）
    /// </summary>
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

    // 导航属性（基于外键）
    public virtual Cabin? Cabin { get; set; }
    public virtual Guest? Guest { get; set; }


    /// <summary>
    /// 办理入住
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public Result CheckIn()
    {
        if (Status != BookingStatus.UnConfirmed)
        {
            return Result.Fail("Only unconfirmed bookings can be checked in.");
        }
        Status = BookingStatus.CheckedIn;
        //入住后默认设置为已支付
        IsPaid = true;
        return Result.Ok();
    }

    /// <summary>
    /// 办理退房
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public Result CheckOut()
    {
        if (Status != BookingStatus.CheckedIn)
        {
            return Result.Fail("Only checked-in bookings can be checked out.");
        }
        Status = BookingStatus.CheckedOut;
        return Result.Ok();
    }

    /// <summary>
    /// 取消入住
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void UnConfirm()
    {
        Status = BookingStatus.UnConfirmed;
        //取消入住后默认设置为未支付
        IsPaid = false;
    }
}


/// <summary>
/// 预订状态枚举
/// </summary>
public enum BookingStatus
{
    /// <summary>
    /// 未确认
    /// </summary>
    UnConfirmed,

    /// <summary>
    /// 已入住
    /// </summary>
    CheckedIn,

    /// <summary>
    /// 已退房
    /// </summary>
    CheckedOut,
}