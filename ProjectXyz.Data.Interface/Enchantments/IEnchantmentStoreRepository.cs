﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

using ProjectXyz.Data.Interface.Enchantments.Contracts;

namespace ProjectXyz.Data.Interface.Enchantments
{
    [ContractClass(typeof(IEnchantmentStoreRepositoryContract))]
    public interface IEnchantmentStoreRepository
    {
        #region Methods
        void Add(IEnchantmentStore enchantmentStore);

        void RemoveById(Guid id);

        IEnchantmentStore GetById(Guid id);
        #endregion
    }
}
