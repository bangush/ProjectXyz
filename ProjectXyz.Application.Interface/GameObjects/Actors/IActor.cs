﻿using System.Diagnostics.Contracts;
using ProjectXyz.Application.Interface.GameObjects.Actors.Contracts;
using ProjectXyz.Application.Interface.GameObjects.Items;
using ProjectXyz.Application.Interface.Items;
using ProjectXyz.Data.Interface.Stats;

namespace ProjectXyz.Application.Interface.GameObjects.Actors
{
    [ContractClass(typeof(IActorContract))]
    public interface IActor : 
        IGameObject,
        ICanEquip, 
        ICanUnequip, 
        ICanUseItem
    {
        #region Properties
        /// <summary>
        /// Gets the X-coordinate of the <see cref="IActor"/> within the game world
        /// </summary>
        float X { get; }

        /// <summary>
        /// Gets the Y-coordinate of the <see cref="IActor"/> within the game world
        /// </summary>
        float Y { get; }

        /// <summary>
        /// Gets the name of the resource used for animations.
        /// </summary>
        string AnimationResource { get; }

        IObservableEquipment Equipment { get; }

        IMutableInventory Inventory { get; }

        IStatCollection Stats { get; }
        #endregion

        #region Methods
        void UpdatePosition(float x, float y);
        #endregion
    }
}