namespace Oasis.Domain;

public class Cabin
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Name { get; set; }
    public short? MaxCapacity { get; set; }
    public short? RegularPrice { get; set; }
    public short? Discount { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
}
