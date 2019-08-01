namespace TestOkur.Notification.Models
{
	public class Template
	{
		public static readonly Template DailyReportEmailAdmin =
			new Template(
				"GunlukRapor_Yonetici_Email",
				"TestOkur Gunluk Veriler",
				"Admin_Daily_Notification.html");

		public static readonly Template UserErrorAlertEmail =
			new Template(
				"UserErrorAlertEmail",
				"Yeni Kullanici Hata Mesaji Iletildi",
				"NewErrorMessageReceived.html");

		public static readonly Template AccountRegistrationEmailAdmin =
			new Template(
				"YeniLisansKaydi_Yonetici_Email",
				"Yeni Lisans Kaydı",
				"Admin_New_User_Mail.html");

		public static readonly Template SmsFailureEmailAdmin =
			new Template(
				"SmsHatasi_Yonetici_Email",
				"SMS Hatasi",
				"Admin_SMS_Failure_Mail.html");

		public static readonly Template AccountExpiryNotificationEmailUser =
			new Template(
				"LisansSonaErmeBildirim_Kullanici_Email",
				"TestOkur Lisans Süreniz Dolmak Üzere",
				"User_License_Expiration_Reminder_Mail.html");

		public static readonly Template AccountExpiryNotificationSmsUser =
			new Template(
				"LisansSonaErmeBildirim_Kullanici_Sms",
				"TestOkur",
				"User_License_Expiration_Reminder_SMS.html");

		public static readonly Template AccountExtensionEmailUser =
			new Template(
				"LisanUzatma_Kullanici_Email",
				"Lisans Yenileme",
				"User_LicenseExtension_Mail.html");

		public static readonly Template AccountExtensionSmsUser =
			new Template(
				"LisanUzatma_Kullanici_Sms",
				"TestOkur",
				"User_LicenseExtension_SMS.html");

		public static readonly Template AccountActivationSmsUser =
			new Template(
				"LisansAktiflestirme_Kullanici_Sms",
				"TestOkur",
				"User_MembershipActivation_SMS.html");

		public static readonly Template AccountActivationEmailUser =
			new Template(
				"LisansAktiflestirme_Kullanici_Email",
				"Lisans Bilgileriniz",
				"User_MembershipActivation_Mail.html");

		public static readonly Template AccountRegistrationEmailUser =
			new Template(
				"YeniLisansKaydi_Kullanici_Email",
				"Lisans Kaydınız Alındı",
				"User_New_User_Mail.html");

		public static readonly Template PasswordReminderEmailUser =
			new Template(
				"ParolaHatirlatma_Kullanici_Email",
				"TestOkur Parolaniz",
				"User_RemindPassword_Mail.html");

		public static readonly Template PasswordResetEmailUser =
			new Template(
				"ParolaSifirlama_Kullanici_Email",
				"TestOkur Parola Sifirla",
				"User_ResetPassword_Mail.html");

		public static readonly Template SmsCreditAddedEmailUser =
			new Template(
				"SmsPaketi_Kullanici_Email",
				"SMS Paketiniz",
				"User_SMSOrder_Mail.html");

		public static readonly Template SmsCreditAddedSmsUser =
			new Template(
				"SmsPaketi_Kullanici_Sms",
				"TestOkur",
				"User_SMSOrder_SMS.html");

		public Template(string name, string subject, string bodyPath)
		{
			Name = name;
			Subject = subject;
			BodyPath = bodyPath;
		}

		public string Name { get; }

		public string Subject { get; }

		public string BodyPath { get; }
	}
}
