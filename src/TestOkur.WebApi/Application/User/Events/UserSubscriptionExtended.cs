namespace TestOkur.WebApi.Application.User.Events
{
    using System;
    using TestOkur.Contracts;
    using TestOkur.Contracts.User;

    public class UserSubscriptionExtended : IntegrationEvent, IUserSubscriptionExtended
    {
        public UserSubscriptionExtended(string firstName, string lastName, string email, string phone, DateTime expiryDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            ExpiryDate = expiryDate;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string Phone { get; }

        public DateTime ExpiryDate { get; }
    }
}
