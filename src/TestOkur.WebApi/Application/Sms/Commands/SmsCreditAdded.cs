namespace TestOkur.WebApi.Application.Sms.Commands
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.Sms;

    public class SmsCreditAdded : IntegrationEvent, ISmsCreditAdded
    {
        public SmsCreditAdded(int amount, int totalSmsCredits, string firstName, string lastName, string email, string phone, bool gift, int userId, string userSubjectId)
        {
            Amount = amount;
            TotalSmsCredits = totalSmsCredits;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Gift = gift;
            UserId = userId;
            UserSubjectId = userSubjectId;
        }

        public int UserId { get; }

        public string UserSubjectId { get; }

        public int Amount { get; }

        public int TotalSmsCredits { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string Phone { get; }

        public bool Gift { get; }
    }
}
