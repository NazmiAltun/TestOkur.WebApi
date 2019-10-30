namespace TestOkur.WebApi.Application.Classroom
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;
    using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;

    public sealed class DeleteClassroomCommandHandler : RequestHandlerAsync<DeleteClassroomCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteClassroomCommandHandler(IPublishEndpoint publishEndpoint, IApplicationDbContextFactory dbContextFactory)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _dbContextFactory = dbContextFactory;
        }

        [ClearCache(2)]
        public override async Task<DeleteClassroomCommand> HandleAsync(
            DeleteClassroomCommand command,
            CancellationToken cancellationToken = default)
        {
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var classroom = await GetClassroomAsync(dbContext, command, cancellationToken);

                if (classroom != null)
                {
                    dbContext.Remove(classroom);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    await PublishEventAsync(command.ClassroomId, cancellationToken);
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task PublishEventAsync(int id, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(
                new ClassroomDeleted(id),
                cancellationToken);
        }

        private Task<Classroom> GetClassroomAsync(
            ApplicationDbContext dbContext,
            DeleteClassroomCommand command,
            CancellationToken cancellationToken)
        {
            return dbContext.Classrooms.FirstOrDefaultAsync(
                l => l.Id == command.ClassroomId && EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
