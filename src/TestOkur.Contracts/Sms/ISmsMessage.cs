namespace TestOkur.Contracts.Sms
{
    using System;

    public interface ISmsMessage
    {
		Guid Id { get; }

		string Subject { get; }

		string Body { get; }

		string Receiver { get; }

		int Credit { get; }

		string StudentOpticalFormId { get; }

		int ExamId { get; }
    }
}
