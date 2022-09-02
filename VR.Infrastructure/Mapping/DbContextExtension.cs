using Mapster;
using Microsoft.EntityFrameworkCore;

namespace VR.Infrastructure.Mapping
{
    public static class DbContextExtension
    {
        public static TDestination MapTo<TSource, TDestination>(this DbContext dbContext,
                                                                       TSource sourceIntance,
                                                                       TDestination destinationInstance)
        {
            return sourceIntance.BuildAdapter().EntityFromContext(dbContext).AdaptTo(destinationInstance);
        }
    }
}
