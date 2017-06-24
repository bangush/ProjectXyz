﻿using ProjectXyz.Application.Enchantments.Api.Calculations;
using ProjectXyz.Framework.Entities.Interface;

namespace ProjectXyz.Plugins.Enchantments.Calculations.State
{
    public sealed class ContextToExpressionInterceptorConverter : IContextToExpressionInterceptorConverter
    {
        private readonly IStateExpressionInterceptorFactory _stateExpressionInterceptorFactory;

        public ContextToExpressionInterceptorConverter(IStateExpressionInterceptorFactory stateExpressionInterceptorFactory)
        {
            _stateExpressionInterceptorFactory = stateExpressionInterceptorFactory;
        }

        public IEnchantmentExpressionInterceptor Convert(IEnchantmentCalculatorContext enchantmentCalculatorContext)
        {
            var stateContextProvider = enchantmentCalculatorContext.GetFirst<IStateContextProvider>();
            var stateEnchantmentExpressionInterceptor = _stateExpressionInterceptorFactory.Create(
                stateContextProvider,
                enchantmentCalculatorContext.Enchantments);
            return stateEnchantmentExpressionInterceptor;
        }
    }
}