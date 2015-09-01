﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace ProjectXyz.Data.Interface.Enchantments.Contracts
{
    [ContractClassFor(typeof(IEnchantmentTriggerFactory))]
    public abstract class IEnchantmentTriggerFactoryContract : IEnchantmentTriggerFactory
    {
        #region Methods
        public IEnchantmentTrigger Create(Guid id, Guid nameStringResourceId)
        {
            Contract.Requires<ArgumentException>(id != Guid.Empty);
            Contract.Requires<ArgumentException>(nameStringResourceId != Guid.Empty);
            Contract.Ensures(Contract.Result<IEnchantmentTrigger>() != null);

            return default(IEnchantmentTrigger);
        }
        #endregion
    }
}
