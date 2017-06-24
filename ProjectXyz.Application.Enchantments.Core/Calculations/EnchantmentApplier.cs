using System.Collections.Generic;
using ProjectXyz.Application.Enchantments.Api.Calculations;
using ProjectXyz.Application.Enchantments.Interface.Calculations;
using ProjectXyz.Framework.Interface;
using ProjectXyz.Framework.Interface.Collections;

namespace ProjectXyz.Application.Enchantments.Core.Calculations
{
    public sealed class EnchantmentApplier : IEnchantmentApplier
    {
        private readonly IEnchantmentCalculator _enchantmentCalculator;


        public EnchantmentApplier(IEnchantmentCalculator enchantmentCalculator)
        {
            _enchantmentCalculator = enchantmentCalculator;
        }

        public IReadOnlyDictionary<IIdentifier, double> ApplyEnchantments(
            IEnchantmentCalculatorContext enchantmentCalculatorContext,
            IReadOnlyDictionary<IIdentifier, double> baseStats)
        {
            var newStats = baseStats.ToDictionary();

            foreach (var enchantment in enchantmentCalculatorContext.Enchantments)
            {
                var statDefinitionId = enchantment.StatDefinitionId;
                
                var value = _enchantmentCalculator.Calculate(
                    enchantmentCalculatorContext,
                    newStats,
                    statDefinitionId);
                newStats[statDefinitionId] = value;
            }

            return newStats;
        }
    }
}