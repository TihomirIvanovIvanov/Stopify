using Stopify.Data.Models;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using System.Reflection;

namespace Stopify.Tests.Common
{
    public static class MapperInitializer
    {
        public static void InitialzeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ProductServiceModel).GetTypeInfo().Assembly,
                typeof(Product).GetTypeInfo().Assembly);
        }
    }
}
