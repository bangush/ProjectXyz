﻿using Autofac;
using ProjectXyz.Framework.Autofac;
using ProjectXyz.Game.Core.GameObjects;

namespace ProjectXyz.Game.Core.Dependencies.Autofac
{
    public sealed class GameObjectsModule : SingleRegistrationModule
    {
        protected override void SafeLoad(ContainerBuilder builder)
        {
            builder
                .RegisterType<GameObjectManager>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}