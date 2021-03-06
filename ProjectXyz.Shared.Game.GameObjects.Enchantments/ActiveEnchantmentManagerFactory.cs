using System.Collections.Generic;
using System.Linq;
using ProjectXyz.Api.Enchantments;
using ProjectXyz.Api.Enchantments.Triggering;
using ProjectXyz.Api.Triggering;

namespace ProjectXyz.Shared.Game.GameObjects.Enchantments
{
    public sealed class ActiveEnchantmentManagerFactory : IActiveEnchantmentManagerFactory
    {
        private readonly ITriggerMechanicRegistrar _triggerMechanicRegistrar;
        private readonly IReadOnlyCollection<IEnchantmentTriggerMechanicRegistrar> _enchantmentTriggerMechanicRegistrars;

        public ActiveEnchantmentManagerFactory(
            ITriggerMechanicRegistrarFacade triggerMechanicRegistrarFacade,
            IEnumerable<IEnchantmentTriggerMechanicRegistrar> enchantmentTriggerMechanicRegistrars)
        {
            _triggerMechanicRegistrar = triggerMechanicRegistrarFacade;
            _enchantmentTriggerMechanicRegistrars = enchantmentTriggerMechanicRegistrars.ToArray();
        }

        public IActiveEnchantmentManager Create()
        {
            return new ActiveEnchantmentManager(
                _triggerMechanicRegistrar,
                _enchantmentTriggerMechanicRegistrars);
        }
    }
}