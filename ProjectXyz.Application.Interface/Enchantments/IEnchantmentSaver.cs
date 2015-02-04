﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectXyz.Application.Interface.Enchantments
{
    public interface IEnchantmentSaver : ISave<IEnchantment, ProjectXyz.Data.Interface.Enchantments.IEnchantmentStore>
    {
    }
}
