﻿using System;
using ProjectXyz.Api.Behaviors;
using ProjectXyz.Api.Framework.Events;
using ProjectXyz.Plugins.Features.CommonBehaviors.Api;
using ProjectXyz.Shared.Game.Behaviors;

namespace ProjectXyz.Plugins.Features.CommonBehaviors
{
    public sealed class ApplySocketEnchantmentsBehavior :
        BaseBehavior,
        IApplySocketEnchantmentsBehavior
    {
        private ICanBeSocketedBehavior _canBeSocketed;

        protected override void OnRegisteredToOwner(IHasBehaviors owner)
        {
            base.OnRegisteredToOwner(owner);

            if (owner.Behaviors.TryGetFirst(out _canBeSocketed))
            {
                _canBeSocketed.Socketed += CanBeSocketed_Socketed;
            }
        }

        private void CanBeSocketed_Socketed(
            object sender,
            EventArgs<Tuple<ICanBeSocketedBehavior, ICanFitSocketBehavior>> e)
        {
            if (e.Data.Item1.Owner.Behaviors.TryGetFirst(out IBuffableBehavior buffable) &&
                e.Data.Item2.Owner.Behaviors.TryGetFirst(out IHasEnchantmentsBehavior hasEnchantments))
            {
                buffable.AddEnchantments(hasEnchantments.Enchantments);
            }
        }
    }
}