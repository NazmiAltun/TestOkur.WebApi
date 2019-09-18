﻿namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Lesson.Queries;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    public sealed class EditLessonCommandHandler : RequestHandlerAsync<EditLessonCommand>
    {
        private readonly IProcessor _processor;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public EditLessonCommandHandler(
            IPublishEndpoint publishEndpoint,
            IQueryProcessor queryProcessor,
            IProcessor processor,
            IApplicationDbContextFactory dbContextFactory)
        {
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
            _processor = processor;
            _dbContextFactory = dbContextFactory;
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<EditLessonCommand> HandleAsync(
            EditLessonCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureLessonDoesNotExistAsync(command, cancellationToken);
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var lesson = await GetAsync(dbContext, command, cancellationToken);

                if (lesson != null)
                {
                    lesson.SetName(command.NewName);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }

            await PublishEventAsync(command, cancellationToken);
            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task PublishEventAsync(
            EditLessonCommand command,
            CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(
                new LessonNameChanged(command.LessonId, command.NewName),
                cancellationToken);
        }

        private async Task EnsureLessonDoesNotExistAsync(
            EditLessonCommand command,
            CancellationToken cancellationToken)
        {
            await EnsureNotExistsInSharedAsync(command, cancellationToken);
            await EnsureNotExistsInUserLessons(command, cancellationToken);
        }

        private async Task EnsureNotExistsInUserLessons(EditLessonCommand command, CancellationToken cancellationToken)
        {
            var lessonsByUserQuery = new GetUserLessonsQuery(command.UserId);
            var lessonsByUser = await _processor
                .ExecuteAsync<GetUserLessonsQuery, IReadOnlyCollection<LessonReadModel>>(lessonsByUserQuery, cancellationToken);

            if (lessonsByUser.Any(l => string.Equals(l.Name, command.NewName, StringComparison.InvariantCultureIgnoreCase) &&
                                       l.Id != command.LessonId))
            {
                throw new ValidationException(ErrorCodes.LessonExists);
            }
        }

        private async Task EnsureNotExistsInSharedAsync(EditLessonCommand command, CancellationToken cancellationToken)
        {
            var sharedQuery = new GetSharedLessonQuery();
            var sharedLessons = await _queryProcessor
                .ExecuteAsync(sharedQuery, cancellationToken);

            if (sharedLessons.Any(l => string.Equals(l.Name, command.NewName, StringComparison.InvariantCultureIgnoreCase) &&
                                       l.Id != command.LessonId))
            {
                throw new ValidationException(ErrorCodes.LessonExists);
            }
        }

        private async Task<Lesson> GetAsync(
            ApplicationDbContext dbContext,
            EditLessonCommand command,
            CancellationToken cancellationToken)
        {
            return await dbContext.Lessons.FirstOrDefaultAsync(
                l => l.Id == command.LessonId &&
                     EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
