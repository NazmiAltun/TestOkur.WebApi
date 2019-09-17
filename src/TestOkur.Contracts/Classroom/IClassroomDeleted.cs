namespace TestOkur.Contracts.Classroom
{
    public interface IClassroomDeleted : IIntegrationEvent
    {
        int ClassroomId { get; }
    }
}
