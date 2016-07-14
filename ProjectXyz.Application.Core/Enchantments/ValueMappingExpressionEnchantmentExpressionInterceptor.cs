﻿using System.Collections.Generic;
using System.Linq;
using ProjectXyz.Application.Interface.Enchantments;
using ProjectXyz.Framework.Interface;

namespace ProjectXyz.Application.Core.Enchantments
{
    public sealed class ValueMappingExpressionEnchantmentExpressionInterceptor : IEnchantmentExpressionInterceptor
    {
        private readonly IReadOnlyDictionary<string, double> _termToValueMapping;

        public ValueMappingExpressionEnchantmentExpressionInterceptor(IReadOnlyDictionary<string, double> termToValueMapping)
        {
            _termToValueMapping = termToValueMapping;
        }

        public string Intercept(
            IIdentifier statDefinitionId,
            string expression)
        {
            expression = _termToValueMapping
                .Aggregate(
                    expression, 
                    (current, termToValue) => current.Replace(
                        termToValue.Key, 
                        $"({termToValue.Value})"));
            return expression;
        }
    }
}