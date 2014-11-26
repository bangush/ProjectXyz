﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectXyz.Data.Interface.Enchantments
{
    public interface IMagicTypesRandomAffixes
    {
        #region Properties
        Guid Id { get; }

        Guid MagicTypeId { get; }

        int MinimumAffixes { get; }

        int MaximumAffixes { get; }
        #endregion
    }
}
