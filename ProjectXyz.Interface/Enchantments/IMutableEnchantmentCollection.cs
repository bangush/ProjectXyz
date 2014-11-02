﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace ProjectXyz.Interface.Enchantments
{
    [ContractClass(typeof(IMutableEnchantmentCollectionContract))]
    public interface IMutableEnchantmentCollection : IEnchantmentCollection
    {
        #region Methods
        void Add(IEnchantment enchantment);

        void AddRange(IEnumerable<IEnchantment> enchantments);

        void Remove(IEnchantment enchantment);

        void RemoveRange(IEnumerable<IEnchantment> enchantments);

        void Clear();
        #endregion
    }
}
