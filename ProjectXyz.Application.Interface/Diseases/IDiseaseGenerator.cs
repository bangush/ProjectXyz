﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectXyz.Application.Interface.Diseases
{
    public interface IDiseaseGenerator
    {
        #region Methods
        IDisease GenerateForId(IRandom randomizer, Guid id);
        #endregion
    }
}
