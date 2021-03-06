﻿using System.Diagnostics;
using ProjectXyz.Api.GameObjects.Generation.Attributes;

namespace ProjectXyz.Shared.Game.GameObjects.Generation.Attributes
{
    [DebuggerDisplay("({Minimum}, {Maximum})")]
    public sealed class RangeGeneratorAttributeValue : IGeneratorAttributeValue
    {
        public RangeGeneratorAttributeValue(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public double Minimum { get; }

        public double Maximum { get; }
    }
}