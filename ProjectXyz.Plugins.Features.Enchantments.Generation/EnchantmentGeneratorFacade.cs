﻿using System;
using System.Collections.Generic;
using System.Linq;

using ProjectXyz.Api.Enchantments;
using ProjectXyz.Api.Enchantments.Generation;
using ProjectXyz.Api.Framework;
using ProjectXyz.Api.Framework.Collections;
using ProjectXyz.Api.GameObjects.Generation;
using ProjectXyz.Api.GameObjects.Generation.Attributes;

namespace ProjectXyz.Plugins.Features.Enchantments.Generation
{
    public sealed class EnchantmentGeneratorFacade : IEnchantmentGeneratorFacade
    {
        private readonly List<IEnchantmentGenerator> _enchantmentGenerators;
        private readonly IAttributeFilterer _attributeFilterer;
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public EnchantmentGeneratorFacade(
            IAttributeFilterer attributeFilterer,
            IRandomNumberGenerator randomNumberGenerator)
        {
            _attributeFilterer = attributeFilterer;
            _randomNumberGenerator = randomNumberGenerator;
            _enchantmentGenerators = new List<IEnchantmentGenerator>();
        }

        public IEnumerable<IEnchantment> GenerateEnchantments(IGeneratorContext generatorContext)
        {
            if (!_enchantmentGenerators.Any())
            {
                throw new InvalidOperationException(
                    $"There are no enchantment generators registered to this " +
                    $"facade. Did you forget to call '{nameof(Register)}()'?");
            }

            var filteredGenerators = _attributeFilterer.Filter(
                _enchantmentGenerators,
                generatorContext);
            var generator = filteredGenerators.RandomOrDefault(_randomNumberGenerator);
            if (generator == null)
            {
                return Enumerable.Empty<IEnchantment>();
            }

            var generatedEnchantments = generator.GenerateEnchantments(generatorContext);
            return generatedEnchantments;
        }

        public IEnumerable<IGeneratorAttribute> SupportedAttributes => _enchantmentGenerators
            .SelectMany(x => x.SupportedAttributes)
            .Distinct();

        public void Register(IEnchantmentGenerator enchantmentGenerator)
        {
            _enchantmentGenerators.Add(enchantmentGenerator);
        }
    }
}