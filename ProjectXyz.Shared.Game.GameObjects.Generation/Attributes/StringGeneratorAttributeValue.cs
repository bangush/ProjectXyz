﻿using System.Diagnostics;
using ProjectXyz.Api.GameObjects.Generation.Attributes;

namespace ProjectXyz.Shared.Game.GameObjects.Generation.Attributes
{
    [DebuggerDisplay("{Value}")]
    public sealed class StringGeneratorAttributeValue : IGeneratorAttributeValue
    {
        public StringGeneratorAttributeValue(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}