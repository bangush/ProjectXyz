﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectXyz.Application.Interface.Items
{
    public interface IEquipment : IItemCollection, IUpdateElapsedTime
    {
        #region Properties
        IItem this[string slot] { get; }
        #endregion
    }
}