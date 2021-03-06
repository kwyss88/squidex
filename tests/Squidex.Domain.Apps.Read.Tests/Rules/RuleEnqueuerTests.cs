﻿// ==========================================================================
//  RuleEnqueuerTests.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using NodaTime;
using Squidex.Domain.Apps.Core.HandleRules;
using Squidex.Domain.Apps.Core.Rules;
using Squidex.Domain.Apps.Core.Rules.Actions;
using Squidex.Domain.Apps.Core.Rules.Triggers;
using Squidex.Domain.Apps.Events.Contents;
using Squidex.Domain.Apps.Read.Rules.Repositories;
using Squidex.Infrastructure;
using Squidex.Infrastructure.CQRS.Events;
using Xunit;

namespace Squidex.Domain.Apps.Read.Rules
{
    public class RuleEnqueuerTests
    {
        private readonly IRuleRepository ruleRepository = A.Fake<IRuleRepository>();
        private readonly IRuleEventRepository ruleEventRepository = A.Fake<IRuleEventRepository>();
        private readonly RuleService ruleService = A.Fake<RuleService>();
        private readonly Instant now = SystemClock.Instance.GetCurrentInstant();
        private readonly NamedId<Guid> appId = new NamedId<Guid>(Guid.NewGuid(), "my-app");
        private readonly RuleEnqueuer sut;

        public RuleEnqueuerTests()
        {
            sut = new RuleEnqueuer(
                ruleEventRepository,
                ruleRepository,
                ruleService);
        }

        [Fact]
        public void Should_return_contents_filter_for_events_filter()
        {
            Assert.Equal(".*", sut.EventsFilter);
        }

        [Fact]
        public void Should_return_type_name_for_name()
        {
            Assert.Equal(typeof(RuleEnqueuer).Name, sut.Name);
        }

        [Fact]
        public Task Should_do_nothing_on_clear()
        {
            return sut.ClearAsync();
        }

        [Fact]
        public async Task Should_update_repositories_on_with_jobs_from_sender()
        {
            var @event = Envelope.Create(new ContentCreated { AppId = appId });

            var rule1 = new Rule(new ContentChangedTrigger(), new WebhookAction { Url = new Uri("https://squidex.io") });
            var rule2 = new Rule(new ContentChangedTrigger(), new WebhookAction { Url = new Uri("https://squidex.io") });
            var rule3 = new Rule(new ContentChangedTrigger(), new WebhookAction { Url = new Uri("https://squidex.io") });

            var job1 = new RuleJob { Created = now };
            var job2 = new RuleJob { Created = now };

            var ruleEntity1 = A.Fake<IRuleEntity>();
            var ruleEntity2 = A.Fake<IRuleEntity>();
            var ruleEntity3 = A.Fake<IRuleEntity>();

            A.CallTo(() => ruleEntity1.Rule).Returns(rule1);
            A.CallTo(() => ruleEntity2.Rule).Returns(rule2);
            A.CallTo(() => ruleEntity3.Rule).Returns(rule3);

            A.CallTo(() => ruleRepository.QueryCachedByAppAsync(appId.Id))
                .Returns(new List<IRuleEntity> { ruleEntity1, ruleEntity2, ruleEntity3 });

            A.CallTo(() => ruleService.CreateJob(rule1, @event))
                .Returns(job1);

            A.CallTo(() => ruleService.CreateJob(rule2, @event))
                .Returns(job2);

            A.CallTo(() => ruleService.CreateJob(rule3, @event))
                .Returns(null);

            await sut.On(@event);

            A.CallTo(() => ruleEventRepository.EnqueueAsync(job1, now))
                .MustHaveHappened();

            A.CallTo(() => ruleEventRepository.EnqueueAsync(job2, now))
                .MustHaveHappened();
        }
    }
}