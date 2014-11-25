﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProjectXyz.Data.Interface.Enchantments;
using ProjectXyz.Data.Interface.Enchantments.ExtensionMethods;
using System.Diagnostics.Contracts;

namespace ProjectXyz.Data.Core.Enchantments
{
    public sealed class Enchantment : IEnchantment
    {
        #region Constructors
        private Enchantment()
        {
        }
        #endregion

        #region Properties
        public Guid StatId
        {
            get;
            set;
        }

        public double Value
        {
            get;
            set;
        }

        public Guid CalculationId
        {
            get;
            set;
        }

        public Guid TriggerId
        {
            get;
            set;
        }

        public Guid StatusTypeId
        {
            get;
            set;
        }

        public TimeSpan RemainingDuration
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public static IEnchantment Create()
        {
            Contract.Ensures(Contract.Result<IEnchantment>() != null);
            return new Enchantment();
        }
        #endregion
    }
}
