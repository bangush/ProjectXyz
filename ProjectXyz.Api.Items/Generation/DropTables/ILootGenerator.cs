﻿using System.Collections.Generic;
using ProjectXyz.Api.GameObjects;
using ProjectXyz.Api.GameObjects.Generation;

namespace ProjectXyz.Plugins.Features.GameObjects.Items.Api.Generation.DropTables
{
    public interface ILootGenerator
    {
        IEnumerable<IGameObject> GenerateLoot(IGeneratorContext generatorContext);
    }
}