using Mapster;
using Oasis.Domain;

namespace Oasis.Application.DTOs.CabinDto;

public sealed class CreateCabinRequest : IMapFrom<Cabin>
{
    public string? Name { get; set; }
    public short? MaxCapacity { get; set; }
    public short? RegularPrice { get; set; }
    public short? Discount { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
}
