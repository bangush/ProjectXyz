﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectXyz.Data.Interface.Items.Materials
{
    public interface IMaterial
    {
        #region Properties
        Guid Id { get; }

        Guid StringResourceId { get; }
        #endregion
    }
}
