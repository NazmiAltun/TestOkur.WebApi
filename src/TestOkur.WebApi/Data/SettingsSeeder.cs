namespace TestOkur.WebApi.Data
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using TestOkur.Common;
	using TestOkur.Data;
	using TestOkur.Domain.Model.SettingModel;

	internal class SettingsSeeder : ISeeder
    {
	    private AppSetting AdminPhones =>
		    new AppSetting(
			    AppSettings.AdminPhones,
			    "5074011191;5052647544",
			    "Yönetici telefon numaraları.Her bir numaradan sonra ';' eklemek gerekiyor.");

	    private AppSetting AccountExpirationNotificationTime =>
		    new AppSetting(
			    AppSettings.AccountExpirationNotificationTime,
			    "20:00",
			    "Kullanicilara 'TestOkur Lisans Süreniz Dolmak Üzere' baslikli e-posta ve smsi gonderen isin calisma zamani.");

	    private AppSetting AccountExpirationNotificationDayInterval =>
		    new AppSetting(
			    AppSettings.AccountExpirationNotificationDayInterval,
			    "7",
			    "Kullanicilara gonderilen 'TestOkur Lisans Süreniz Dolmak Üzere' e-posta ve smsi lisans bitiminden kac gun once gonderilecek degerine karsilik gelir.");

	    private AppSetting SystemAdminEmails =>
		    new AppSetting(
			    AppSettings.SystemAdminEmails,
                "nazmialtun@windowslive.com;necatiyalcin@gmail.com;fuatkayadelen@hotmail.com",
			    "Sistem yoneticisi e-posta adresi.Her bir e-posta adresinden sonra ';' eklemek gerekiyor");

	    private AppSetting DailyJobRunTime =>
		    new AppSetting(
			    AppSettings.DailyJobRunTime,
			    "23:55",
			    "Sistem yoneticilerine 'TestOkur Gunluk Veriler' baslikli e-postayi gonderen isin calisma zamani.");

	    private AppSetting AdminEmails =>
		    new AppSetting(
			    AppSettings.AdminEmails,
			    "nazmialtun@windowslive.com;necatiyalcin@gmail.com;fuatkayadelen@hotmail.com",
			    "Yönetici e-posta adresi.Her bir e-posta adresinden sonra ';' eklemek gerekiyor");

	    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
	    {
		    if (!await dbContext.AppSettings.AnyAsync())
		    {
			    var appSettings = new List<AppSetting>
			    {
				    AdminEmails,
				    AdminPhones,
				    DailyJobRunTime,
				    AccountExpirationNotificationTime,
				    AccountExpirationNotificationDayInterval,
				    SystemAdminEmails,
			    };
			    dbContext.AppSettings.AddRange(appSettings);
			    await dbContext.SaveChangesAsync();
		    }
	    }
	}
}
