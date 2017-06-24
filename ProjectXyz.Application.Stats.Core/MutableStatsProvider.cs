using System;
using System.Collections.Generic;
using ProjectXyz.Application.Stats.Interface;
using ProjectXyz.Framework.Interface;
using ProjectXyz.Framework.Interface.Collections;

namespace ProjectXyz.Application.Stats.Core
{
    public sealed class MutableStatsProvider : IMutableStatsProvider
    {
        private readonly Dictionary<IIdentifier, double> _stats;

        public MutableStatsProvider()
            : this(new Dictionary<IIdentifier, double>())
        {
        }

        public MutableStatsProvider(IEnumerable<KeyValuePair<IIdentifier, double>> stats)
        {
            _stats = stats.ToDictionary();
        }

        public IReadOnlyDictionary<IIdentifier, double> Stats => _stats;

        public void UsingMutableStats(Action<IDictionary<IIdentifier, double>> callback)
        {
            callback(_stats);
        }
    }
}