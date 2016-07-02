﻿using System.Collections.Generic;
using ProjectXyz.Application.Interface.Stats;
using ProjectXyz.Framework.Interface;

namespace ProjectXyz.Application.Interface.Enchantments
{
    public interface IEnchantmentCalculator
    {
        double Calculate(
            IReadOnlyDictionary<IIdentifier, IStat> baseStats,
            IReadOnlyCollection<IEnchantment> enchantments,
            IIdentifier statDefinitionId);
    }
}