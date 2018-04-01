using ProjectXyz.Api.Framework;
using ProjectXyz.Api.Systems;

namespace ProjectXyz.Plugins.Features.TimeOfDay
{
    public interface ITimeOfDaySystem : ISystem
    {
        IIdentifier TimeOfDay { get; }
    }
}