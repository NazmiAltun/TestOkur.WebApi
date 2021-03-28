﻿namespace TestOkur.WebApi.Integration.Tests.Lesson
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Lesson;
    using Xunit;

    public class GetTests : LessonTest
    {
       [Fact(Skip = "Fix later")]
        public async Task Given_GetSharedLessons_Should_Return_ListofSharedLessons()
        {
            var client = (await GetTestServer()).CreateClient();
            var response = await client.GetAsync($"{ApiPath}/shared");
            var lessons = await response.ReadAsync<IEnumerable<LessonReadModel>>();

            lessons.Should().NotContain(l => !l.Shared);
            lessons.Should().HaveCountGreaterOrEqualTo(15);
        }
    }
}
