namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Lesson.Queries;

    public sealed class CreateLessonCommandHandler : RequestHandlerAsync<CreateLessonCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        private readonly IQueryProcessor _queryProcessor;

        public CreateLessonCommandHandler(
            IQueryProcessor queryProcessor,
            IApplicationDbContextFactory dbContextFactory)
        {
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
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
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
            var lessonsByUser = await _queryProcessor.ExecuteAsync(lessonsByUserQuery, cancellationToken);

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
