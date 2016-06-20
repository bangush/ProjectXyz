﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary1.Application.Core.Weather;
using ClassLibrary1.Application.Interface.Enchantments;
using ClassLibrary1.Application.Interface.Stats;
using ClassLibrary1.Application.Interface.Weather;
using ClassLibrary1.Framework.Interface.Collections;

namespace ClassLibrary1.Plugins.Enchantments.Expressions
{
    public sealed class EnchantmentTypeCalculator : IEnchantmentTypeCalculator
    {
        #region Fields
        private readonly IStatFactory _statFactory;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IEnchantmentTypeCalculatorResultFactory _enchantmentTypeCalculatorResultFactory;
        private readonly IStatCollectionFactory _statCollectionFactory;
        private readonly IWeatherManager _weatherManager;
        #endregion

        #region Constructors
        public EnchantmentTypeCalculator(
            IStatFactory statFactory,
            IExpressionEvaluator expressionEvaluator,
            IEnchantmentTypeCalculatorResultFactory enchantmentTypeCalculatorResultFactory,
            IStatCollectionFactory statCollectionFactory,
            IWeatherManager weatherManager)
        {
            _statFactory = statFactory;
            _expressionEvaluator = expressionEvaluator;
            _enchantmentTypeCalculatorResultFactory = enchantmentTypeCalculatorResultFactory;
            _statCollectionFactory = statCollectionFactory;
            _weatherManager = weatherManager;
        }
        #endregion

        #region Methods
        public IEnchantmentTypeCalculatorResult Calculate(
            IEnchantmentContext enchantmentContext,
            IStatCollection stats,
            IEnumerable<IEnchantment> enchantments)
        {
            var processResult = ProcessEnchantments(
                enchantmentContext,
                enchantments,
                stats);
            var newStats = _statCollectionFactory.Create(processResult.Item2);
            var processedEnchantments = processResult.Item1;

            var result = _enchantmentTypeCalculatorResultFactory.Create(
                new IEnchantment[0],
                new IEnchantment[0],
                processedEnchantments,
                newStats);
            return result;
        }

        private Tuple<List<IEnchantment>, List<IStat>>  ProcessEnchantments(
            IEnchantmentContext enchantmentContext,
            IEnumerable<IEnchantment> enchantments,
            IStatCollection stats)
        {
            var processedEnchantments = new List<IEnchantment>();
            var newStats = new List<IStat>();

            var activeEnchantments = GetActiveExpressionEnchantments(
                enchantmentContext,
                enchantments);

            foreach (var enchantment in activeEnchantments)
            {
                var newValue = _expressionEvaluator.Evaluate(
                    enchantment,
                    stats);
                var newStat = _statFactory.Create(
                    enchantment.StatDefinitionId,
                    newValue);

                newStats.Add(newStat);
                processedEnchantments.Add(enchantment);
            }

            return new Tuple<List<IEnchantment>, List<IStat>>(
                processedEnchantments,
                newStats);
        }

        private IEnumerable<IExpressionEnchantment> GetActiveExpressionEnchantments(
            IEnchantmentContext enchantmentContext,
            IEnumerable<IEnchantment> enchantments)
        {
            return enchantments
                .TakeTypes<IEnchantment, IExpressionEnchantment>()
                .Where(x => _weatherManager.WeatherGroupingContainsWeatherDefinition(
                    x.WeatherGroupingId,
                    enchantmentContext.ActiveWeatherDefinitionId))
                .OrderBy(x => x.CalculationPriority);
        }
        #endregion
    }
}
