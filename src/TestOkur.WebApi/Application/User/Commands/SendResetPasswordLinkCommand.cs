namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.Cqrs;

    [DataContract]
    public class SendResetPasswordLinkCommand : CommandBase
	{
		public SendResetPasswordLinkCommand(
			Guid id,
			string email,
			Guid captchaId,
			string captchaCode)
			: base(id)
		{
			Email = email;
			CaptchaId = captchaId;
			CaptchaCode = captchaCode;
		}

		[DataMember]
		public string Email { get; }

		[DataMember]
		public Guid CaptchaId { get; private set; }

		[DataMember]
		public string CaptchaCode { get; private set; }
	}
}
