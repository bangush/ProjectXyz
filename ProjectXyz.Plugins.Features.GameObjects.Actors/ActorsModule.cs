﻿using Autofac;
using ProjectXyz.Framework.Autofac;

namespace ProjectXyz.Plugins.Features.GameObjects.Actors
{
    public sealed class ActorsModule : SingleRegistrationModule
    {
        protected override void SafeLoad(ContainerBuilder builder)
        {
            builder
                .RegisterType<ActorFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<Actor>(); // delegate factory
            builder
                .RegisterType<NoAdditionalActorBehaviorsProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}