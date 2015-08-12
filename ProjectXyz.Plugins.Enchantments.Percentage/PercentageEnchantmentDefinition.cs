﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ProjectXyz.Data.Interface.Enchantments;

namespace ProjectXyz.Plugins.Enchantments.Percentage
{
    public sealed class PercentageEnchantmentDefinition : IPercentageEnchantmentDefinition
    {
        #region Constructors
        private PercentageEnchantmentDefinition(
            Guid id,
            Guid statId,
            Guid triggerId,
            Guid statusTypeId,
            double minimumValue,
            double maximumValue,
            TimeSpan minimumDuration,
            TimeSpan maximumDuration)
        {
            Contract.Requires<ArgumentOutOfRangeException>(minimumValue <= maximumValue);
            Contract.Requires<ArgumentOutOfRangeException>(minimumDuration <= maximumDuration);

            this.Id = id;
            this.StatId = statId;
            this.TriggerId = triggerId;
            this.StatusTypeId = statusTypeId;
            this.MinimumValue = minimumValue;
            this.MaximumValue = maximumValue;
            this.MinimumDuration = minimumDuration;
            this.MaximumDuration = maximumDuration;
        }
        #endregion

        #region Properties
        public Guid Id
        {
            get;
            set;
        }

        public Guid StatId
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

        public TimeSpan MinimumDuration
        {
            get;
            set;
        }

        public TimeSpan MaximumDuration
        {
            get;
            set;
        }

        public double MinimumValue
        {
            get;
            set;
        }

        public double MaximumValue
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public static IEnchantmentDefinition Create(
            Guid id,
            Guid statId,
            Guid triggerId,
            Guid statusTypeId,
            double minimumValue,
            double maximumValue,
            TimeSpan minimumDuration,
            TimeSpan maximumDuration)
        {
            Contract.Requires<ArgumentOutOfRangeException>(minimumValue <= maximumValue);
            Contract.Requires<ArgumentOutOfRangeException>(minimumDuration <= maximumDuration);
            Contract.Ensures(Contract.Result<IEnchantmentDefinition>() != null);

            return new PercentageEnchantmentDefinition(
                id,
                statId,
                triggerId,
                statusTypeId,
                minimumValue,
                maximumValue,
                minimumDuration,
                maximumDuration);
        }
        #endregion
    }
}
