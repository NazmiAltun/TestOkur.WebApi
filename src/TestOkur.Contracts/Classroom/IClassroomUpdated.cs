namespace TestOkur.Contracts.Classroom
{
    public interface IClassroomUpdated : IIntegrationEvent
    {
        int ClassroomId { get; }

        int Grade { get; }

        string Name { get; }
    }
}
