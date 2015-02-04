﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectXyz.Application.Interface.Items
{
    public interface IMutableItemCollection : IItemCollection
    {
        #region Methods
        void Add(IEnumerable<IItem> items);
        
        bool Remove(IEnumerable<IItem> items);
        
        void Clear();
        #endregion
    }
}
