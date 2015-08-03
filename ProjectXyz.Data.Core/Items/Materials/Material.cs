﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

using ProjectXyz.Data.Interface.Items.Materials;

namespace ProjectXyz.Data.Core.Items.Materials
{
    public sealed class Material : IMaterial
    {
        #region Constructors
        private Material(Guid materialId, Guid stringResourceId)
        {
            Contract.Requires<ArgumentException>(materialId != Guid.Empty);
            Contract.Requires<ArgumentException>(stringResourceId != Guid.Empty);

            Id = materialId;
            StringResourceId = stringResourceId;
        }
        #endregion

        #region Properties
        public Guid Id
        {
            get;
            private set;
        }

        public Guid StringResourceId
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public static IMaterial Create(Guid materialId, Guid stringResourceId)
        {
            Contract.Requires<ArgumentException>(materialId != Guid.Empty);
            Contract.Requires<ArgumentException>(stringResourceId != Guid.Empty);
            Contract.Ensures(Contract.Result<IMaterial>() != null);
            return new Material(materialId, stringResourceId);
        }
        #endregion
    }
}