namespace TestOkur.WebApi.Application.User.Events
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.User;
    using TestOkur.Domain.Model.UserModel;

    public class ReferredUserActivated : IntegrationEvent, IReferredUserActivated
    {
        public ReferredUserActivated(
            User referee,
            User referrer,
            int refereeGainedSmsCredits,
            int referrerGainedSmsCredits)
        {
            ReferrerUserId = (int)referrer.Id;
            RefereeUserId = (int)referee.Id;
            ReferrerSubjectId = referrer.SubjectId;
            RefereeSubjectId = referee.SubjectId;
            RefereeEmail = referee.Email;
            RefereeFirstName = referee.FirstName;
            RefereeLastName = referee.LastName;
            RefereePhone = referee.Phone;
            RefereeSmsBalance = referee.SmsBalance;
            ReferrerEmail = referrer.Email;
            ReferrerFirstName = referrer.FirstName;
            ReferrerLastName = referrer.LastName;
            ReferrerPhone = referrer.Phone;
            ReferrerSmsBalance = referrer.SmsBalance;
            RefereeGainedSmsCredits = refereeGainedSmsCredits;
            ReferrerGainedSmsCredits = referrerGainedSmsCredits;
        }

        public string RefereeFirstName { get; }

        public string RefereeLastName { get; }

        public string RefereeEmail { get; }

        public string RefereePhone { get; }

        public string ReferrerFirstName { get; }

        public string ReferrerLastName { get; }

        public string ReferrerEmail { get; }

        public string ReferrerPhone { get; }

        public int ReferrerGainedSmsCredits { get; }

        public int ReferrerSmsBalance { get; }

        public int RefereeGainedSmsCredits { get; }

        public int RefereeSmsBalance { get; }

        public int RefereeUserId { get; }

        public string RefereeSubjectId { get; }

        public int ReferrerUserId { get; }

        public string ReferrerSubjectId { get; }
    }
}
