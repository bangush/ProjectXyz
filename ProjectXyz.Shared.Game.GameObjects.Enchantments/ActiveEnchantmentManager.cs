using System;
using System.Collections.Generic;
using System.Linq;
using ProjectXyz.Api.Enchantments;
using ProjectXyz.Api.Triggering;
using ProjectXyz.Plugins.Features.ExpiringEnchantments.Api;

namespace ProjectXyz.Shared.Game.GameObjects.Enchantments
{
    public sealed class ActiveEnchantmentManager : IActiveEnchantmentManager
    {
        private readonly Dictionary<IEnchantment, List<ITriggerMechanic>> _activeEnchantments;
        private readonly ITriggerMechanicRegistrar _triggerMechanicRegistrar;
        private readonly IReadOnlyCollection<IEnchantmentTriggerMechanicRegistrar> _enchantmentTriggerMechanicRegistrars;

        public ActiveEnchantmentManager(
            ITriggerMechanicRegistrar triggerMechanicRegistrar, 
            IEnumerable<IEnchantmentTriggerMechanicRegistrar> enchantmentTriggerMechanicRegistrars)
        {
            _activeEnchantments = new Dictionary<IEnchantment, List<ITriggerMechanic>>();
            _triggerMechanicRegistrar = triggerMechanicRegistrar;
            _enchantmentTriggerMechanicRegistrars = enchantmentTriggerMechanicRegistrars.ToArray();
        }

        public IReadOnlyCollection<IEnchantment> Enchantments => _activeEnchantments.Keys;

        public void Add(IEnumerable<IEnchantment> enchantments)
        {
            foreach (var enchantment in enchantments)
            {
                if (!_activeEnchantments.ContainsKey(enchantment))
                {
                    _activeEnchantments[enchantment] = new List<ITriggerMechanic>();
                }

                // TODO: this should be registered up as some sort of
                // "interceptor". all of this code belongs in the plugin/feature
                // domain. please see:
                // https://bitbucket.org/nexuslabs/projectxyz/issues/49
                foreach (var enchantmentTriggerMechanicRegistrar in _enchantmentTriggerMechanicRegistrars)
                {
                    var triggers = enchantmentTriggerMechanicRegistrar.RegisterToEnchantment(
                        enchantment,
                        RemoveTriggerMechanicFromEnchantment);
                    foreach (var trigger in triggers)
                    {
                        _activeEnchantments[enchantment].Add(trigger);

                        if (_triggerMechanicRegistrar.CanRegister(trigger))
                        {
                            _triggerMechanicRegistrar.RegisterTrigger(trigger);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Could not register '{trigger}' to '{_triggerMechanicRegistrar}'.");
                        }
                    }
                }
            }
        }

        public void Remove(IEnumerable<IEnchantment> enchantments)
        {
            foreach (var enchantment in enchantments)
            {
                foreach (var triggerMechanic in _activeEnchantments[enchantment])
                {
                    _triggerMechanicRegistrar.UnregisterTrigger(triggerMechanic);
                }

                _activeEnchantments.Remove(enchantment);
            }
        }

        private void RemoveTriggerMechanicFromEnchantment(IEnchantment enchantment, ITriggerMechanic triggerMechanic)
        {
            if (_activeEnchantments[enchantment].Count == 1)
            {
                _activeEnchantments.Remove(enchantment);
            }
            else if (!_activeEnchantments[enchantment].Remove(triggerMechanic))
            {
                throw new InvalidOperationException($"Attempted to remove trigger '{triggerMechanic}' but the collection did not contain it.");
            }
        }
    }
}