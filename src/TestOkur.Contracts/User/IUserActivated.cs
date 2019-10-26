namespace TestOkur.Contracts.User
{
    public interface IUserActivated : IIntegrationEvent
    {
        string FirstName { get; }

        string LastName { get; }

        string Email { get; }

        string Phone { get; }

        int UserId { get; }

        string UserSubjectId { get; }
    }
}
