﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

using ProjectXyz.Data.Interface.Enchantments.Contracts;

namespace ProjectXyz.Data.Interface.Enchantments
{
    [ContractClass(typeof(IMagicTypesRandomAffixesFactoryContract))]
    public interface IMagicTypesRandomAffixesFactory
    {
        #region Methods
        IMagicTypesRandomAffixes CreateMagicTypesRandomAffixes(Guid id, Guid magicTypeId, int minimumAffixes, int maximumAffixes);
        #endregion
    }
}
