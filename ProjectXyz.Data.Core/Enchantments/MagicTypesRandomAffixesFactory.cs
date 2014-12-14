﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

using ProjectXyz.Data.Interface.Enchantments;

namespace ProjectXyz.Data.Core.Enchantments
{
    public class MagicTypesRandomAffixesFactory : IMagicTypesRandomAffixesFactory
    {
        #region Constructors
        private MagicTypesRandomAffixesFactory()
        {
        }
        #endregion

        #region Methods
        public static IMagicTypesRandomAffixesFactory Create()
        {
            Contract.Ensures(Contract.Result<IMagicTypesRandomAffixesFactory>() != null);

            return new MagicTypesRandomAffixesFactory();
        }

        public IMagicTypesRandomAffixes CreateMagicTypesRandomAffixes(Guid id, Guid magicTypeId, int minimumAffixes, int maximumAffixes)
        {
            return MagicTypesRandomAffixes.Create(id, magicTypeId, minimumAffixes, maximumAffixes);
        }
        #endregion
    }
}