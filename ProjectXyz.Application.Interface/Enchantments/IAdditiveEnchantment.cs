﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectXyz.Application.Interface.Enchantments
{
    public interface IAdditiveEnchantment : IEnchantment
    {
        #region Properties
        Guid StatId { get; }

        double Value { get; }
        #endregion
    }
}