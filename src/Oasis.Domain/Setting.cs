namespace Oasis.Domain;

public class Setting
{
    public long Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public short? MinBookingLength { get; set; }
    public short? MaxBookingLength { get; set; }
    public short? MaxGuestsPerBooking { get; set; }
    public float? BreakfastPrice { get; set; }
}