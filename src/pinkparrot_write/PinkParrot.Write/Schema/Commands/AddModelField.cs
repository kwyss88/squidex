﻿// ==========================================================================
//  AddModelField.cs
//  PinkParrot Headless CMS
// ==========================================================================
//  Copyright (c) PinkParrot Group
//  All rights reserved.
// ==========================================================================

using PinkParrot.Infrastructure.CQRS.Commands;

namespace PinkParrot.Write.Schema.Commands
{
    public class AddModelField : AggregateCommand
    {
        public string FieldType;

        public string FieldName;
    }
}