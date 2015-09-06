﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ProjectXyz.Data.Interface.Items.MagicTypes;

namespace ProjectXyz.Data.Core.Items.MagicTypes
{
    public sealed class MagicTypeFactory : IMagicTypeFactory
    {
        #region Constructors
        private MagicTypeFactory()
        {
        }
        #endregion

        #region Methods
        public static IMagicTypeFactory Create()
        {
            var factory = new MagicTypeFactory();
            return factory;
        }

        public IMagicType Create(
            Guid id,
            Guid nameStringResourceId)
        {
            Contract.Requires<ArgumentException>(id != Guid.Empty);
            Contract.Requires<ArgumentException>(nameStringResourceId != Guid.Empty);
            Contract.Ensures(Contract.Result<IMagicType>() != null);
            
            var magicType = MagicType.Create(
                id,
                nameStringResourceId);
            return magicType;
        }
        #endregion
    }
}