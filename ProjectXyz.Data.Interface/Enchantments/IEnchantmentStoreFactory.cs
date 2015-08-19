﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ProjectXyz.Data.Interface.Enchantments.Contracts;

namespace ProjectXyz.Data.Interface.Enchantments
{
    public interface IEnchantmentStoreFactory
    {
        #region Methods
        IEnchantmentStore Create(
            Guid id,
            Guid triggerId,
            Guid statusTypeId,
            Guid enchantmentTypeId,
            Guid enchantmentWeatherId,
            TimeSpan remainingDuration);
        #endregion
    }
}
