namespace TestOkur.Notification.Dtos
{
    using MongoDB.Bson;
    using System;
    using System.Collections.Generic;
    using TestOkur.Contracts.Sms;
    using TestOkur.Contracts.User;

    public class SmsLog
    {
        public SmsLog(int userId, string userSubjectId, int amount, SmsLogType logType, DateTime dateTimeUtc)
        {
            UserId = userId;
            UserSubjectId = userSubjectId;
            Amount = amount;
            LogType = logType;
            DateTimeUtc = dateTimeUtc;
        }

        public SmsLog()
        {
        }

        public ObjectId Id { get; set; }

        public int UserId { get; private set; }

        public string UserSubjectId { get; private set; }

        public int Amount { get; private set; }

        public SmsLogType LogType { get; private set; }

        public DateTime DateTimeUtc { get; private set; }

        public static SmsLog CreateSmsAdditionLog(ISmsCreditAdded @event)
        {
            return new SmsLog(
                @event.UserId,
                @event.UserSubjectId,
                @event.Amount,
                @event.Gift ? SmsLogType.Gift : SmsLogType.Purchase,
                @event.CreatedOnUTC);
        }

        public static IEnumerable<SmsLog> CreateSmsPromotionLogs(IReferredUserActivated @event)
        {
            yield return new SmsLog(
                @event.RefereeUserId,
                @event.RefereeSubjectId,
                @event.RefereeGainedSmsCredits,
                SmsLogType.Promotion,
                @event.CreatedOnUTC);

            yield return new SmsLog(
                @event.ReferrerUserId,
                @event.ReferrerSubjectId,
                @event.ReferrerGainedSmsCredits,
                SmsLogType.Promotion,
                @event.CreatedOnUTC);
        }

        public static SmsLog CreateUsageLog(ISendSmsRequestReceived @event)
        {
            return new SmsLog(
                @event.UserId,
                @event.UserSubjectId,
                @event.CreditAmount,
                SmsLogType.UsageDeduction,
                @event.CreatedOnUTC);
        }

        public static SmsLog CreateInitialLog(IUserActivated @event)
        {
            return new SmsLog(
                @event.UserId,
                @event.UserSubjectId,
                0,
                SmsLogType.Initial,
                DateTime.UtcNow);
        }
    }
}