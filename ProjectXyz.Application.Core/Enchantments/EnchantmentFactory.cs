﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ProjectXyz.Application.Interface.Enchantments;
using ProjectXyz.Data.Interface.Enchantments;

namespace ProjectXyz.Application.Core.Enchantments
{
    public sealed class EnchantmentFactory : IEnchantmentFactory
    {
        #region Fields
        private readonly Dictionary<Type, CreateEnchantmentDelegate> _createEnchantmentMapping;
        #endregion

        #region Constructors
        private EnchantmentFactory()
        {
            _createEnchantmentMapping = new Dictionary<Type, CreateEnchantmentDelegate>();
        }
        #endregion

        #region Methods
        public static IEnchantmentFactory Create()
        {
            Contract.Ensures(Contract.Result<IEnchantmentFactory>() != null);

            return new EnchantmentFactory();
        }

        public void RegisterCallbackForType<TSpecificType>(CreateEnchantmentDelegate callbackToRegister)
            where TSpecificType : IEnchantment
        {
            RegisterCallbackForType(typeof(TSpecificType), callbackToRegister);
        }

        public void RegisterCallbackForType(Type type, CreateEnchantmentDelegate callbackToRegister)
        {
            _createEnchantmentMapping[type] = callbackToRegister;
        }

        public IEnchantment Create(IEnchantmentStore enchantmentStore)
        {
            var enchantmentType = enchantmentStore.GetType();
            if (!_createEnchantmentMapping.ContainsKey(enchantmentType))
            {
                throw new InvalidOperationException(string.Format("No callback registered for type '{0}'.", enchantmentType));
            }

            var enchantment = _createEnchantmentMapping[enchantmentType].Invoke(enchantmentStore);
            return enchantment;
        }
        #endregion
    }
}
