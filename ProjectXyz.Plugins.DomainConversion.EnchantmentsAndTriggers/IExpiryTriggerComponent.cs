using ProjectXyz.Api.Framework.Entities;

namespace ProjectXyz.Plugins.Triggers.Enchantments.Expiration
{
    public interface IExpiryTriggerComponent : IExpiryComponent
    {
        IComponent TriggerComponent { get; }
    }
}