﻿using System;
using System.Collections;
using System.Collections.Generic;
using Autofac;
using ProjectXyz.Api.GameObjects.Generation.Attributes;
using ProjectXyz.Game.Core.Autofac;
using ProjectXyz.Shared.Framework;
using ProjectXyz.Shared.Game.GameObjects.Generation;
using ProjectXyz.Shared.Game.GameObjects.Generation.Attributes;
using Xunit;

namespace ProjectXyz.Plugins.Features.GameObjects.Generation.InMemory.Tests
{
    public sealed class InMemoryAttributeFiltererFunctionalTests
    {
        private readonly IAttributeFilterer _inMemoryAttributeFilterer;

        public InMemoryAttributeFiltererFunctionalTests()
        {
            var moduleDiscoverer = new ModuleDiscoverer();
            var modules = moduleDiscoverer.Discover(
                AppDomain.CurrentDomain.BaseDirectory,
                "*.dll");
            var dependencyContainerBuilder = new DependencyContainerBuilder();
            var container = dependencyContainerBuilder.Create(modules);

            _inMemoryAttributeFilterer = container.Resolve<IAttributeFilterer>();
        }

        [ClassData(typeof(TestData))]
        [Theory]
        private void Filter_SourceWithAttributes_Expected(
            string name,
            IReadOnlyCollection<IHasGeneratorAttributes> source,
            IReadOnlyCollection<IGeneratorAttribute> attributes,
            IReadOnlyCollection<IHasGeneratorAttributes> expectedResults)
        {
            var generatorContext = new GeneratorContext(
                1,
                1,
                attributes);

            var results = _inMemoryAttributeFilterer.Filter(
                source,
                generatorContext);

            Assert.Equal(results, expectedResults);
        }

        private sealed class TestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return CreateEmptySourceEmptyFilter();
                yield return CreateSingleComponentNoSupportedAttributesEmptyFilter();
                yield return CreateMultipleComponentNoSupportedAttributesEmptyFilter();
                yield return CreateMatchingDoubleComponent();
                yield return CreateMatchingDoubleComponentIncludesAttributeless();
                yield return CreateSupportedComponentsNotInContextNotRequired();
                yield return CreateSupportedComponentsNotInContextRequired();
                yield return CreateRequiredMatchingDoubleComponentExcludesAttributeless();
                yield return CreateInRangeComponent();
                yield return CreateInRangeComponentIncludesAttributeless();
                yield return CreateMatchInRangeComponent();
                yield return CreateMatchInRangeComponentIncludesAttributeless();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private object[] CreateEmptySourceEmptyFilter()
            {
                return new object[]
                {
                    "Empty Source, Empty Attributes",
                    new IHasGeneratorAttributes[0],
                    new IGeneratorAttribute[0],
                    new IHasGeneratorAttributes[0],
                };
            }

            private object[] CreateSingleComponentNoSupportedAttributesEmptyFilter()
            {
                var expectedComponent = new GeneratorComponent();

                return new object[]
                {
                    "Single Component, No Supported Attributes, Empty Filter",
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                    },
                    new IGeneratorAttribute[]
                    {

                    },
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                    },
                };
            }

            private object[] CreateMultipleComponentNoSupportedAttributesEmptyFilter()
            {
                var expectedComponent = new GeneratorComponent();
                var expectedComponent2 = new GeneratorComponent();
                var expectedComponent3 = new GeneratorComponent();

                return new object[]
                {
                    "Multiple Components, No Supported Attributes, Empty Filter",
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                        expectedComponent3,
                    },
                    new IGeneratorAttribute[]
                    {

                    },
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                        expectedComponent3,
                    },
                };
            }

            private object[] CreateMatchingDoubleComponent()
            {
                var component = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(1),
                        false)
                });
                var component2 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(2),
                        false)
                });
                var expectedComponent = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(3),
                        false)
                });

                return new object[]
                {
                    "Filter Matching Double Component",
                    new IHasGeneratorAttributes[]
                    {
                        component,
                        expectedComponent,
                        component2
                    },
                    new IGeneratorAttribute[]
                    {
                        new GeneratorAttribute(
                            new StringIdentifier("id"),
                            new DoubleGeneratorAttributeValue(3),
                            false)
                    },
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                    },
                };
            }

            private object[] CreateMatchingDoubleComponentIncludesAttributeless()
            {
                var expectedComponent = new GeneratorComponent();
                var expectedComponent2 = new GeneratorComponent();
                var expectedComponent3 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(3),
                        false)
                });

                return new object[]
                {
                    "Filter With Double Match, Includes Attributeless",
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                        expectedComponent3
                    },
                    new IGeneratorAttribute[]
                    {
                        new GeneratorAttribute(
                            new StringIdentifier("id"),
                            new DoubleGeneratorAttributeValue(3),
                            false)
                    },
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                        expectedComponent3
                    },
                };
            }

            private object[] CreateSupportedComponentsNotInContextNotRequired()
            {
                var expectedComponent = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(1),
                        false)
                });
                var expectedComponent2 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(2),
                        false)
                });
                var expectedComponent3 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(3),
                        false)
                });

                return new object[]
                {
                    "Supported Components Not In Context, Not Required",
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                        expectedComponent3,
                    },
                    new IGeneratorAttribute[]
                    {
                    },
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                        expectedComponent3,
                    },
                };
            }

            private object[] CreateSupportedComponentsNotInContextRequired()
            {
                var component = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(1),
                        true)
                });
                var component2 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(2),
                        true)
                });
                var component3 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(3),
                        true)
                });

                return new object[]
                {
                    "Supported Components Not In Context, Required",
                    new IHasGeneratorAttributes[]
                    {
                        component,
                        component2,
                        component3,
                    },
                    new IGeneratorAttribute[]
                    {
                    },
                    new IHasGeneratorAttributes[]
                    {
                    },
                };
            }

            private object[] CreateRequiredMatchingDoubleComponentExcludesAttributeless()
            {
                var component = new GeneratorComponent();
                var component2 = new GeneratorComponent();
                var expectedComponent3 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(3),
                        false)
                });

                return new object[]
                {
                    "Filter With Required Double Match, Excludes Attributeless",
                    new IHasGeneratorAttributes[]
                    {
                        component,
                        component2,
                        expectedComponent3
                    },
                    new IGeneratorAttribute[]
                    {
                        new GeneratorAttribute(
                            new StringIdentifier("id"),
                            new DoubleGeneratorAttributeValue(3),
                            true)
                    },
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent3
                    },
                };
            }

            private object[] CreateInRangeComponent()
            {
                var component = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(1),
                        false)
                });
                var expectedComponent = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(2),
                        false)
                });
                var expectedComponent2 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(3),
                        false)
                });

                return new object[]
                {
                    "Filter In Range",
                    new IHasGeneratorAttributes[]
                    {
                        component,
                        expectedComponent,
                        expectedComponent2,
                    },
                    new IGeneratorAttribute[]
                    {
                        new GeneratorAttribute(
                            new StringIdentifier("id"),
                            new RangeGeneratorAttributeValue(2, 3), 
                            false)
                    },
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                    },
                };
            }

            private object[] CreateInRangeComponentIncludesAttributeless()
            {
                var expectedComponent = new GeneratorComponent();
                var expectedComponent2 = new GeneratorComponent();
                var expectedComponent3 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new DoubleGeneratorAttributeValue(3),
                        false)
                });

                return new object[]
                {
                    "Filter In Range, Includes Match Plus Attributeless",
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                        expectedComponent3
                    },
                    new IGeneratorAttribute[]
                    {
                        new GeneratorAttribute(
                            new StringIdentifier("id"),
                            new RangeGeneratorAttributeValue(1, 4),
                            false)
                    },
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                        expectedComponent3
                    },
                };
            }

            private object[] CreateMatchInRangeComponent()
            {
                var component = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new RangeGeneratorAttributeValue(1, 9),
                        false)
                });
                var expectedComponent = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new RangeGeneratorAttributeValue(5, 10),
                        false)
                });
                var expectedComponent2 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new RangeGeneratorAttributeValue(10, 20),
                        false)
                });

                return new object[]
                {
                    "Filter Matching In Range",
                    new IHasGeneratorAttributes[]
                    {
                        component,
                        expectedComponent,
                        expectedComponent2,
                    },
                    new IGeneratorAttribute[]
                    {
                        new GeneratorAttribute(
                            new StringIdentifier("id"),
                            new DoubleGeneratorAttributeValue(10), 
                            false)
                    },
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                    },
                };
            }

            private object[] CreateMatchInRangeComponentIncludesAttributeless()
            {
                var expectedComponent = new GeneratorComponent();
                var expectedComponent2 = new GeneratorComponent();
                var expectedComponent3 = new GeneratorComponent(new[]
                {
                    new GeneratorAttribute(
                        new StringIdentifier("id"),
                        new RangeGeneratorAttributeValue(5, 15),
                        false)
                });

                return new object[]
                {
                    "Filter Match In Range, Includes Match Plus Attributeless",
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                        expectedComponent3
                    },
                    new IGeneratorAttribute[]
                    {
                        new GeneratorAttribute(
                            new StringIdentifier("id"),
                            new DoubleGeneratorAttributeValue(10), 
                            false)
                    },
                    new IHasGeneratorAttributes[]
                    {
                        expectedComponent,
                        expectedComponent2,
                        expectedComponent3
                    },
                };
            }
        }
    }
}