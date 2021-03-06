﻿using System;
using System.Linq;

namespace ProjectXyz.Api.Behaviors
{
    public static class IBehaviorCollectionExtensions
    {
        public static TBehavior GetFirst<TBehavior>(this IBehaviorCollection behaviors)
            where TBehavior : IBehavior
        {
            var behavior = behaviors
                .Get<TBehavior>()
                .FirstOrDefault();
            if (behavior == null)
            {
                throw new InvalidOperationException($"Could not find a behavior of type '{typeof(TBehavior)}'.");
            }

            return behavior;
        }

        public static bool TryGetFirst<TBehavior>(
            this IBehaviorCollection behaviors,
            out TBehavior behavior)
            where TBehavior : IBehavior
        {
            behavior = behaviors
                .Get<TBehavior>()
                .FirstOrDefault();
            return behavior != null;
        }

        public static TBehavior GetOnly<TBehavior>(this IBehaviorCollection behaviors)
            where TBehavior : IBehavior
        {
            return behaviors
                .Get<TBehavior>()
                .Single();
        }

        public static bool Has<TBehavior>(this IBehaviorCollection behaviors)
            where TBehavior : IBehavior
        {
            var behavior = behaviors
                .Get<TBehavior>()
                .FirstOrDefault();
            return behavior != null;
        }
    }
}