namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    public sealed class DeleteLessonCommandHandler : RequestHandlerAsync<DeleteLessonCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public DeleteLessonCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [ClearCache(2)]
        public override async Task<DeleteLessonCommand> HandleAsync(
            DeleteLessonCommand command,
            CancellationToken cancellationToken = default)
        {
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var lesson = await GetAsync(dbContext, command, cancellationToken);

                if (lesson != null)
                {
                    dbContext.Remove(lesson);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private Task<Lesson> GetAsync(
            ApplicationDbContext dbContext,
            DeleteLessonCommand command,
            CancellationToken cancellationToken)
        {
            return dbContext.Lessons.FirstOrDefaultAsync(
                l => l.Id == command.LessonId &&
                     EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
