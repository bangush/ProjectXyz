﻿using System.Collections.Generic;
using Autofac;
using ProjectXyz.Api.Data.Serialization;
using ProjectXyz.Api.Framework.Collections;
using ProjectXyz.Framework.Autofac;

namespace ProjectXyz.Plugins.Data.Newtonsoft.Autofac
{
    public sealed class InternalDependenciesModule : SingleRegistrationModule
    {
        protected override void SafeLoad(ContainerBuilder builder)
        {
            builder
                .RegisterType<DtoJsonSerializer>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder
                .RegisterBuildCallback(x =>
                {
                    x
                        .Resolve<IEnumerable<IDiscoverableSerializableDtoDataConverter>>()
                        .Foreach(d => SerializableDtoDataConverterService.Instance.Register(
                            d.DeserializableType,
                            d));
                });
            builder
                .RegisterType<SerializableConverterFacade>()
                .AsImplementedInterfaces()
                .SingleInstance()
                .OnActivated(x =>
                {
                    x
                     .Context
                     .Resolve<IEnumerable<IDiscoverableSerializableConverter>>()
                     .Foreach(d => x.Instance.Register(
                         d.Type,
                         d.DtoType,
                         d));
                });
        }
    }
}
