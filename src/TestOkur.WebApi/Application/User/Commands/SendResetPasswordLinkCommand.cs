namespace TestOkur.WebApi.Application.User.Commands
{
    using Paramore.Brighter;
    using System;

    public class SendResetPasswordLinkCommand : Command
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

        public SendResetPasswordLinkCommand()
            : base(Guid.NewGuid())
        {
        }

        public string Email { get; set; }

        public Guid CaptchaId { get; set; }

        public string CaptchaCode { get; set; }
    }
}
