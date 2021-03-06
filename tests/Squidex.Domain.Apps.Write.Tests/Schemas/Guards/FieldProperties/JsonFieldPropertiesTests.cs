﻿// ==========================================================================
//  JsonFieldPropertiesTests.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System.Linq;
using Squidex.Domain.Apps.Core.Schemas;
using Xunit;

namespace Squidex.Domain.Apps.Write.Schemas.Guards.FieldProperties
{
    public class JsonFieldPropertiesTests
    {
        [Fact]
        public void Should_add_error_if_editor_is_not_valid()
        {
            var sut = new JsonFieldProperties();

            var errors = FieldPropertiesValidator.Validate(sut).ToList();

            Assert.Empty(errors);
        }
    }
}
