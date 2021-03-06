﻿using Autofac;
using ProjectXyz.Framework.Autofac;
using ProjectXyz.Game.Core.Systems;

namespace ProjectXyz.Game.Core.Dependencies.Autofac
{
    public sealed class SystemsModule : SingleRegistrationModule
    {
        protected override void SafeLoad(ContainerBuilder builder)
        {
            builder
                .RegisterType<GameObjectManagerSystem>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}