﻿// ==========================================================================
//  UpdateModelField.cs
//  PinkParrot Headless CMS
// ==========================================================================
//  Copyright (c) PinkParrot Group
//  All rights reserved.
// ==========================================================================

using PinkParrot.Infrastructure;
using PinkParrot.Infrastructure.CQRS.Commands;

namespace PinkParrot.Write.Schema.Commands
{
    public class UpdateModelField : AggregateCommand
    {
        public long FieldId;

        public PropertiesBag Settings;
    }
}