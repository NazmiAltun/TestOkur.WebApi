namespace TestOkur.Contracts.User
{
    public interface IReferredUserActivated : IIntegrationEvent
    {
        string RefereeFirstName { get; }

        string RefereeLastName { get; }

        string RefereeEmail { get; }

        string RefereePhone { get; }

        string ReferrerFirstName { get; }

        string ReferrerLastName { get; }

        string ReferrerEmail { get; }

        string ReferrerPhone { get; }

        int ReferrerGainedSmsCredits { get; }

        int ReferrerSmsBalance { get; }

        int RefereeGainedSmsCredits { get; }

        int RefereeSmsBalance { get; }
    }
}
