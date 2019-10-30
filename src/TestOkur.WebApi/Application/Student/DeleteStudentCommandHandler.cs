namespace TestOkur.WebApi.Application.Student
{
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;
    using Student = TestOkur.Domain.Model.StudentModel.Student;

    public class DeleteStudentCommandHandler : RequestHandlerAsync<DeleteStudentCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteStudentCommandHandler(IPublishEndpoint publishEndpoint, IApplicationDbContextFactory dbContextFactory)
        {
            _publishEndpoint = publishEndpoint;
            _dbContextFactory = dbContextFactory;
        }

        [ClearCache(2)]
        public override async Task<DeleteStudentCommand> HandleAsync(
            DeleteStudentCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var student = await GetStudentAsync(dbContext, command, cancellationToken);

                if (student != null)
                {
                    dbContext.Remove(student);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    await PublishEventAsync(command.StudentId, cancellationToken);
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task PublishEventAsync(int id, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(
                new StudentDeleted(id),
                cancellationToken);
        }

        private Task<Student> GetStudentAsync(
            ApplicationDbContext dbContext,
            DeleteStudentCommand command,
            CancellationToken cancellationToken)
        {
            return dbContext.Students.FirstOrDefaultAsync(
                l => l.Id == command.StudentId &&
                     EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
