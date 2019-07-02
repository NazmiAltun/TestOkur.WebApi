namespace TestOkur.Notification.Infrastructure
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class SmsException : Exception
	{
		public SmsException()
		{
		}

		public SmsException(string message)
			: base(message)
		{
		}

		public SmsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected SmsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}