namespace TestOkur.Contracts.Student
{
    public interface IStudentUpdated : IIntegrationEvent
    {
        int StudentId { get; }

        string FirstName { get; }

        string LastName { get; }

        int StudentNumber { get; }

        int ClassroomId { get; }
    }
}
