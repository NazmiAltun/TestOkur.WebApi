namespace TestOkur.WebApi.Application.Classroom
{
    using Paramore.Brighter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class CreateClassroomCommandHandler : RequestHandlerAsync<CreateClassroomCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IProcessor _processor;

        public CreateClassroomCommandHandler(
            IApplicationDbContextFactory dbContext,
            IProcessor processor)
        {
            _dbContextFactory = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [Idempotent(2)]
        [ClearCache(4)]
        public override async Task<CreateClassroomCommand> HandleAsync(
            CreateClassroomCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureClassroomDoesNotExistAsync(command, cancellationToken);
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                dbContext.Classrooms.Add(command.ToDomainModel());
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task EnsureClassroomDoesNotExistAsync(
            CreateClassroomCommand command,
            CancellationToken cancellationToken)
        {
            var query = new GetUserClassroomsQuery(command.UserId);
            var list = await _processor.ExecuteAsync<GetUserClassroomsQuery, IReadOnlyCollection<ClassroomReadModel>>(query, cancellationToken);

            if (list.Any(c => c.Grade == command.Grade &&
                             string.Equals(c.Name, command.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ValidationException(ErrorCodes.ClassroomExists);
            }
        }
    }
}
