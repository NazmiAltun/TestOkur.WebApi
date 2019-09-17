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
    using TestOkur.Infrastructure.Cqrs;

    public sealed class CreateClassroomCommandHandler : RequestHandlerAsync<CreateClassroomCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProcessor _processor;

        public CreateClassroomCommandHandler(
            ApplicationDbContext dbContext,
            IProcessor processor)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [Idempotent(2)]
        [ClearCache(4)]
        public override async Task<CreateClassroomCommand> HandleAsync(
            CreateClassroomCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureClassroomDoesNotExistAsync(command, cancellationToken);
            _dbContext.Classrooms.Add(command.ToDomainModel());
            await _dbContext.SaveChangesAsync(cancellationToken);

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task EnsureClassroomDoesNotExistAsync(
            CreateClassroomCommand command,
            CancellationToken cancellationToken)
        {
            var query = new GetUserClassroomsQuery();
            var list = await _processor.ExecuteAsync<GetUserClassroomsQuery, IReadOnlyCollection<ClassroomReadModel>>(query, cancellationToken);

            if (list.Any(c => c.Grade == command.Grade &&
                             string.Equals(c.Name, command.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ValidationException(ErrorCodes.ClassroomExists);
            }
        }
    }
}
