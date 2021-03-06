﻿// ==========================================================================
//  PubSubServices.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Squidex.Infrastructure;
using Squidex.Infrastructure.Log;
using StackExchange.Redis;

namespace Squidex.Config.Domain
{
    public static class PubSubServices
    {
        public static void AddMyPubSubServices(this IServiceCollection services, IConfiguration config)
        {
            config.ConfigureByOption("pubSub:type", new Options
            {
                ["InMemory"] = () =>
                {
                    services.AddSingleton<InMemoryPubSub>()
                        .As<IPubSub>();
                },
                ["Redis"] = () =>
                {
                    var configuration = config.GetRequiredValue("pubsub:redis:configuration");

                    var redis = Singletons<IConnectionMultiplexer>.GetOrAddLazy(configuration, s => ConnectionMultiplexer.Connect(s));

                    services.AddSingleton(c => new RedisPubSub(redis, c.GetRequiredService<ISemanticLog>()))
                        .As<IPubSub>()
                        .As<IExternalSystem>();
                }
            });
        }
    }
}
