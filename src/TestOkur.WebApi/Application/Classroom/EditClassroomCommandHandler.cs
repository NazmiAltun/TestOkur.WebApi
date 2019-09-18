namespace TestOkur.WebApi.Application.Classroom
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Cqrs;
    using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;

    public sealed class EditClassroomCommandHandler : RequestHandlerAsync<EditClassroomCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IProcessor _processor;
        private readonly IPublishEndpoint _publishEndpoint;

        public EditClassroomCommandHandler(
            IProcessor processor,
            IPublishEndpoint publishEndpoint,
            IApplicationDbContextFactory dbContextFactory)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _publishEndpoint = publishEndpoint;
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<EditClassroomCommand> HandleAsync(
            EditClassroomCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureNotExistsAsync(command, cancellationToken);
            using (var dbContext = _dbContextFactory.Create(command.UserId))
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

        private async Task PublishEventAsync(EditClassroomCommand command, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(
                new ClassroomUpdated(command.ClassroomId, command.NewGrade, command.NewName),
                cancellationToken);
        }

        private async Task EnsureNotExistsAsync(
            EditClassroomCommand command,
            CancellationToken cancellationToken)
        {
            var classrooms = await _processor.ExecuteAsync<GetUserClassroomsQuery, IReadOnlyCollection<ClassroomReadModel>>(
                new GetUserClassroomsQuery(),
                cancellationToken);

            if (classrooms.Any(
                c => c.Grade == command.NewGrade &&
                     string.Equals(c.Name, command.NewName, StringComparison.InvariantCultureIgnoreCase) &&
                     c.Id != command.ClassroomId))
            {
                throw new ValidationException(ErrorCodes.ClassroomExists);
            }
        }

        private async Task<Classroom> GetClassroomAsync(
            ApplicationDbContext dbContext,
            EditClassroomCommand command,
            CancellationToken cancellationToken)
        {
            return await dbContext.Classrooms.FirstOrDefaultAsync(
                l => l.Id == command.ClassroomId && EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
