﻿using System;

namespace ProjectXyz.Data.Interface.Items
{
    public interface IItemNamePart
    {
        #region Properties
        Guid Id { get; }

        Guid PartId { get; }

        Guid NameStringResourceId { get; }

        int Order { get; }
        #endregion
    }
}
