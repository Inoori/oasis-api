using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Oasis.Domain;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// OData EDM 模型配置类，定义实体集和导航属性以支持 OData 查询
/// </summary>
public static class EdmModelConfiguration
{
    /// <summary>
    /// 构建 OData EDM 模型，注册实体集和导航属性
    /// </summary>
    /// <returns></returns>
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Cabin>("Cabins");  // 注册 Cabins 实体集
        builder.EntitySet<Guest>("Guests");  // 注册 Guests 实体集
        builder.EntitySet<Booking>("Bookings");  // 注册 Bookings 实体集
        return builder.GetEdmModel();
    }
}