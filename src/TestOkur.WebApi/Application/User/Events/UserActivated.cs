namespace TestOkur.WebApi.Application.User.Events
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.User;

    public class UserActivated : IntegrationEvent, IUserActivated
    {
        public UserActivated(string phone, string email, string lastName, string firstName, int userId, string userSubjectId)
        {
            Phone = phone;
            Email = email;
            LastName = lastName;
            FirstName = firstName;
            UserId = userId;
            UserSubjectId = userSubjectId;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string Phone { get; }

        public int UserId { get; }

        public string UserSubjectId { get; }
    }
}
