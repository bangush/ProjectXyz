﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ProjectXyz.Application.Core.Actors;
using ProjectXyz.Application.Core.Enchantments;
using ProjectXyz.Application.Core.Items;
using ProjectXyz.Application.Core.Maps;
using ProjectXyz.Application.Interface;
using ProjectXyz.Application.Interface.Actors;
using ProjectXyz.Application.Interface.Enchantments;
using ProjectXyz.Application.Interface.Items;
using ProjectXyz.Application.Interface.Maps;
using ProjectXyz.Data.Interface;

namespace ProjectXyz.Application.Core
{
    public sealed class ApplicationManager : IApplicationManager
    {
        #region Fields
        private readonly IActorManager _actorManager;
        private readonly IMapManager _mapManager;
        private readonly IItemApplicationManager _itemManager;
        private readonly IEnchantmentApplicationManager _enchantmentManager;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationManager" /> class.
        /// </summary>
        /// <param name="dataManager">The data store. Cannot be null.</param>
        private ApplicationManager(IDataManager dataManager)
        {
            Contract.Requires<ArgumentNullException>(dataManager != null, "dataManager");

            _actorManager = ActorManager.Create(dataManager.Actors);
            
            _mapManager = MapManager.Create(dataManager.Maps);

            // FIXME: initialize these...
            var enchantmentTypeCalculators = Enumerable.Empty<IEnchantmentTypeCalculator>();
            _enchantmentManager = EnchantmentApplicationManager.Create(
                dataManager,
                enchantmentTypeCalculators);
            
            _itemManager = ItemApplicationManager.Create(
                dataManager,
                _enchantmentManager);
        }
        #endregion

        #region Properties
        public IActorManager Actors
        {
            get { return _actorManager; }
        }

        public IMapManager Maps
        {
            get { return _mapManager; }
        }

        public IItemApplicationManager Items
        {
            get { return _itemManager; }
        }

        public IEnchantmentApplicationManager Enchantments
        {
            get { return _enchantmentManager; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="IApplicationManager" /> instance.
        /// </summary>
        /// <param name="dataManager">The data store. Cannot be null.</param>
        /// <returns>
        /// Returns a new <see cref="IApplicationManager" /> instance.
        /// </returns>
        public static IApplicationManager Create(IDataManager dataManager)
        {
            Contract.Requires<ArgumentNullException>(dataManager != null, "dataManager");
            Contract.Ensures(Contract.Result<IApplicationManager>() != null);

            return new ApplicationManager(dataManager);
        }
        #endregion
    }
}