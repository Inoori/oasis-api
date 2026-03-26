using Mapster;
using Oasis.Domain;

namespace Oasis.Application.DTOs.CabinFeature;

public sealed class CreateCabinRequest : IMapFrom<Cabin>
{
    public string? Name { get; set; }
    public short? MaxCapacity { get; set; }
    public decimal? RegularPrice { get; set; }
    public decimal? Discount { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
}
