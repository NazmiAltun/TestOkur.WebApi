namespace TestOkur.Contracts.Sms
{
    public interface ISmsCreditAdded : IIntegrationEvent
    {
        int UserId { get; }

        string UserSubjectId { get; }

        int Amount { get; }

        int TotalSmsCredits { get; }

        string FirstName { get; }

        string LastName { get; }

        string Email { get;  }

        string Phone { get; }

        bool Gift { get;  }
    }
}
