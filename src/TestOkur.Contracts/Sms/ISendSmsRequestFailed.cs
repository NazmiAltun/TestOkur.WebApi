namespace TestOkur.Contracts.Sms
{
    public interface ISendSmsRequestFailed : IIntegrationEvent
    {
        int UserId { get; }

        string SmsBody { get; }

        string Receiver { get; }

        string ErrorMessage { get; }

        string UserFriendlyMessage { get; }

        string UserEmail { get; }
    }
}
