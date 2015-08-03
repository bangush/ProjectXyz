﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectXyz.Data.Interface.Items.Materials
{
    public interface ISocketTypeFactory
    {
        #region Methods
        IMaterial CreateById(Guid materialId);
        #endregion
    }
}
