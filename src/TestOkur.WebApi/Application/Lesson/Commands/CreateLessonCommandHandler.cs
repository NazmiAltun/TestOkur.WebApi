namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Lesson.Queries;

    public sealed class CreateLessonCommandHandler : RequestHandlerAsync<CreateLessonCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IProcessor _processor;
        private readonly IQueryProcessor _queryProcessor;

        public CreateLessonCommandHandler(
            IProcessor processor,
            IQueryProcessor queryProcessor,
            IApplicationDbContextFactory dbContextFactory)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _queryProcessor = queryProcessor;
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<CreateLessonCommand> HandleAsync(
            CreateLessonCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureLessonDoesNotExistAsync(command, cancellationToken);
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                dbContext.Lessons.Add(command.ToDomainModel());
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task EnsureLessonDoesNotExistAsync(
            CreateLessonCommand command,
            CancellationToken cancellationToken)
        {
            await EnsureNotExistsInSharedAsync(command.Name, cancellationToken);
            await EnsureNotExistsInUserLessons(command, cancellationToken);
        }

        private async Task EnsureNotExistsInUserLessons(CreateLessonCommand command, CancellationToken cancellationToken)
        {
            var lessonsByUserQuery = new GetUserLessonsQuery(command.UserId);
            var lessonsByUser = await _processor
                .ExecuteAsync<GetUserLessonsQuery, IReadOnlyCollection<LessonReadModel>>(lessonsByUserQuery, cancellationToken);

            if (lessonsByUser.Any(l => string.Equals(l.Name, command.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ValidationException(ErrorCodes.LessonExists);
            }
        }

        private async Task EnsureNotExistsInSharedAsync(string name, CancellationToken cancellationToken)
        {
            var sharedQuery = new GetSharedLessonQuery();
            var sharedLessons = await _queryProcessor
                .ExecuteAsync(sharedQuery, cancellationToken);

            if (sharedLessons.Any(l => string.Equals(l.Name, name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ValidationException(ErrorCodes.LessonExists);
            }
        }
    }
}
