﻿using System.Collections.Generic;
using ProjectXyz.Api.GameObjects;
using ProjectXyz.Api.GameObjects.Generation;
using ProjectXyz.Api.GameObjects.Generation.Attributes;
using ProjectXyz.Plugins.Features.GameObjects.Items.Api.Generation;

namespace ConsoleApplication1.Wip.Items.Generation.Plugins
{
    public sealed class AlwaysMatchItemGeneratorPlugin : IItemGenerator
    {
        public IEnumerable<IGameObject> GenerateItems(IGeneratorContext generatorContext)
        {
            yield break;
        }

        public IEnumerable<IGeneratorAttribute> SupportedAttributes { get; } = new IGeneratorAttribute[0];
    }
}