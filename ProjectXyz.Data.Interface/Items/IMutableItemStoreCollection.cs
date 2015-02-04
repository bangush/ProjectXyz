﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectXyz.Data.Interface.Items
{
    public interface IMutableItemStoreCollection : IItemStoreCollection
    {
        #region Methods
        void Add(IEnumerable<IItemStore> items);
        
        bool Remove(IEnumerable<IItemStore> item);
        
        void Clear();
        #endregion
    }
}
