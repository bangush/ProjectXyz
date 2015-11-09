﻿using System;

namespace ProjectXyz.Api.Messaging.Interface.GameObjects.Inventory
{
    public interface ICanAddItemToInventoryRequest : IRequest
    {
        #region Properties
        IInventoryItemPath SourceItemPath { get; }

        IInventoryItemPath DestinationItemPath { get; }
        #endregion
    }
}
