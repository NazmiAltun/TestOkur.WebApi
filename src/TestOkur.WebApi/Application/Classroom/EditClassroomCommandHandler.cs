namespace TestOkur.WebApi.Application.Classroom
{
    using System;
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
    using TestOkur.Infrastructure.CommandsQueries;
    using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;

    public sealed class EditClassroomCommandHandler : RequestHandlerAsync<EditClassroomCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IBus _bus;

        public EditClassroomCommandHandler(
            IBus bus,
            IApplicationDbContextFactory dbContextFactory,
            IQueryProcessor queryProcessor)
        {
            _bus = bus;
            _dbContextFactory = dbContextFactory;
            _queryProcessor = queryProcessor;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<EditClassroomCommand> HandleAsync(
            EditClassroomCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureNotExistsAsync(command, cancellationToken);
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var classroom = await GetClassroomAsync(dbContext, command, cancellationToken);

                if (classroom != null)
                {
                    classroom.Update(command.NewGrade, command.NewName);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    await PublishEventAsync(command, cancellationToken);
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private Task PublishEventAsync(EditClassroomCommand command, CancellationToken cancellationToken)
        {
            return _bus.Publish(
                new ClassroomUpdated(command.ClassroomId, command.NewGrade, command.NewName),
                cancellationToken);
        }

        private async Task EnsureNotExistsAsync(
            EditClassroomCommand command,
            CancellationToken cancellationToken)
        {
            var classrooms = await _queryProcessor.ExecuteAsync(new GetUserClassroomsQuery(command.UserId), cancellationToken);

            if (classrooms.Any(
                c => c.Grade == command.NewGrade &&
                     string.Equals(c.Name, command.NewName, StringComparison.InvariantCultureIgnoreCase) &&
                     c.Id != command.ClassroomId))
            {
                throw new ValidationException(ErrorCodes.ClassroomExists);
            }
        }

        private Task<Classroom> GetClassroomAsync(
            ApplicationDbContext dbContext,
            EditClassroomCommand command,
            CancellationToken cancellationToken)
        {
            return dbContext.Classrooms.FirstOrDefaultAsync(
                l => l.Id == command.ClassroomId && EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
