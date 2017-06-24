using System.Collections.Generic;
using System.Linq;
using Jace;
using ProjectXyz.Api.Enchantments.Calculations;
using ProjectXyz.Application.Core.Triggering;
using ProjectXyz.Application.Core.Triggering.Triggers.Duration;
using ProjectXyz.Application.Core.Triggering.Triggers.Elapsed;
using ProjectXyz.Application.Enchantments.Api;
using ProjectXyz.Application.Enchantments.Core;
using ProjectXyz.Application.Enchantments.Core.Calculations;
using ProjectXyz.Application.Enchantments.Core.Expiration;
using ProjectXyz.Application.Enchantments.Interface;
using ProjectXyz.Application.Enchantments.Interface.Calculations;
using ProjectXyz.Application.Enchantments.Interface.Expiration;
using ProjectXyz.Application.Interface.Triggering;
using ProjectXyz.Application.Stats.Core.Calculations;
using ProjectXyz.Application.Stats.Interface.Calculations;
using ProjectXyz.Framework.Entities.Interface;
using ProjectXyz.Framework.Interface;
using ProjectXyz.Framework.Interface.Collections;
using ProjectXyz.Framework.Shared.Math;
using ProjectXyz.Game.Core.Enchantments;
using ProjectXyz.Game.Tests.Functional.TestingData.Stats;
using ProjectXyz.Plugins.Core;

namespace ProjectXyz.Game.Tests.Functional.TestingData
{
    public sealed class TestFixture
    {
        #region Constructors
        public TestFixture(TestData testData)
        {
            var statDefinitionIdToTermMapping = testData
                .StatsPlugin
                .StatDefinitionToTermMappingRepository
                .GetStatDefinitionIdToTermMappings()
                .ToDictionary(x => x.StateDefinitionId, x => x.Term);

            var statDefinitionIdToCalculationMapping = testData.StatDefinitionIdToCalculationMapping;

            var jaceCalculationEngine = new CalculationEngine();
            var stringExpressionEvaluator = new StringExpressionEvaluatorWrapper(new GenericExpressionEvaluator(jaceCalculationEngine.Calculate), true);

            var statCalculationValueNodeFactory = new StatCalculationValueNodeFactory();
            var statCalculationExpressionNodeFactory = new StatCalculationExpressionNodeFactory(stringExpressionEvaluator);
            var statCalculationNodeFactory = new StatCalculationNodeFactoryWrapper(new IStatCalculationNodeFactory[]
            {
                statCalculationValueNodeFactory,
                statCalculationExpressionNodeFactory
            });

            var expressionStatDefinitionDependencyFinder = new ExpressionStatDefinitionDependencyFinder();

            var statCalculationNodeCreator = new StatCalculationNodeCreator(
                statCalculationNodeFactory,
                expressionStatDefinitionDependencyFinder,
                statDefinitionIdToTermMapping,
                statDefinitionIdToCalculationMapping);

            var statCalculator = new StatCalculator(statCalculationNodeCreator);

            var pluginArgs = new PluginArgs(new IComponent[]
            {
                testData.StatsPlugin.StatDefinitionToTermMappingRepository,
                testData.StatesPlugin.StateIdToTermRepository,
                new ValueMapperRepository(testData.UnitInterval),
                new StatDefinitionIdToBoundsMappingRepository(testData.Stats),
            });

            var enchantmentsAndStatsDcPlugin = new ProjectXyz.Plugins.DomainConversion.EnchantmentsAndStats.Plugin(pluginArgs);
            var enchantmentExpressionInterceptorConverters = enchantmentsAndStatsDcPlugin.EnchantmentExpressionInterceptorConverters;

            var statBoundsPlugin = new ProjectXyz.Plugins.Stats.Calculations.Bounded.Plugin(pluginArgs);
            var statExpressionInterceptors = statBoundsPlugin.StatExpressionInterceptors;

            var enchantmentStatCalculator = new StatCalculatorWrapper(
                statCalculator,
                statExpressionInterceptors,
                enchantmentExpressionInterceptorConverters);
            
            var contextToInterceptorsConverter = new ContextToInterceptorsConverter();
            
            var p1 = new Plugins.Enchantments.Calculations.Expressions.Plugin(pluginArgs);
            var p2 = new Plugins.Enchantments.Calculations.State.Plugin(pluginArgs);

            contextToInterceptorsConverter.Register(p1.ContextToExpressionInterceptorConverter);
            contextToInterceptorsConverter.Register(p2.ContextToExpressionInterceptorConverter);

            EnchantmentCalculator = new EnchantmentCalculator(
                enchantmentStatCalculator,
                contextToInterceptorsConverter);

            EnchantmentApplier = new EnchantmentApplier(EnchantmentCalculator);

            ElapsedTimeTriggerSourceMechanic = new ElapsedTimeTriggerSourceMechanic();

            TriggerMechanicRegistrar = new TriggerMechanicRegistrar(ElapsedTimeTriggerSourceMechanic.AsArray());

            var durationTriggerMechanicFactory = new DurationTriggerMechanicFactory();
            var expiryTriggerMechanicFactory = new ExpiryTriggerMechanicFactory(durationTriggerMechanicFactory);
            ActiveEnchantmentManager = new ActiveEnchantmentManager(
                expiryTriggerMechanicFactory,
                TriggerMechanicRegistrar);
        }
        #endregion

        #region Properties
        public ITriggerMechanicRegistrar TriggerMechanicRegistrar { get; }

        public ElapsedTimeTriggerSourceMechanic ElapsedTimeTriggerSourceMechanic { get; }

        public IActiveEnchantmentManager ActiveEnchantmentManager { get; }

        public IEnchantmentCalculator EnchantmentCalculator { get; }

        public IEnchantmentApplier EnchantmentApplier { get; }
        #endregion
    }

    public sealed class ValueMapperRepository : IValueMapperRepository
    {
        private readonly IInterval _unitInterval;

        public ValueMapperRepository(IInterval unitInterval)
        {
            _unitInterval = unitInterval;
        }

        public IEnumerable<ValueMapperDelegate> GetValueMappers()
        {
            yield return context => new KeyValuePair<string, double>("INTERVAL", context.Elapsed.Divide(_unitInterval));
        }
    }

    public sealed class EnchantmentFactory
    {
        public IEnchantment CreateExpressionEnchantment(
            IIdentifier statDefinitionId,
            string expression,
            ICalculationPriority calculationPriority,
            params IExpiryComponent[] expiry)
        {
            IEnumerable<IComponent> components = new EnchantmentExpressionComponent(
                calculationPriority,
                expression)
                .Yield();

            if (expiry != null)
            {
                components = components.Concat(expiry);
            }

            return new Enchantment(
                statDefinitionId,
                components);
        }
    }
}