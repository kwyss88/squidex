﻿// ==========================================================================
//  ModelFieldShown.cs
//  PinkParrot Headless CMS
// ==========================================================================
//  Copyright (c) PinkParrot Group
//  All rights reserved.
// ==========================================================================

using PinkParrot.Infrastructure.CQRS.Events;

namespace PinkParrot.Events.Schema
{
    public class ModelFieldShown : IEvent
    {
        public long FieldId;
    }
}