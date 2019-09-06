namespace TestOkur.Notification.Unit.Tests.ScheduledTasks
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Hosting;
	using NSubstitute;
	using TestOkur.Notification.Configuration;
	using TestOkur.Notification.Infrastructure;
	using TestOkur.Notification.Infrastructure.Clients;
	using TestOkur.Notification.Models;
	using TestOkur.Notification.ScheduledTasks;
	using Xunit;

	public class SendLicenseExpirationNoticeShould
	{
		[Fact]
		public async Task SendEmailAndSmsToSoonToBeExpiredLicenseOwners()
		{
			var identityUsers = new List<IdentityUser>()
			{
				new IdentityUser
				{
					Active = true,
					Email = "user1@hotmail.com",
					ExpiryDateUtc = null,
					Id = Guid.NewGuid().ToString(),
				},
				new IdentityUser
				{
					Active = true,
					Email = "user2@hotmail.com",
					ExpiryDateUtc = DateTime.UtcNow.AddDays(7),
					Id = Guid.NewGuid().ToString(),
				},
				new IdentityUser
				{
					Active = false,
					Email = "user3@hotmail.com",
					ExpiryDateUtc = DateTime.UtcNow.AddDays(7),
					Id = Guid.NewGuid().ToString(),
				},
				new IdentityUser
				{
					Active = true,
					Email = "user4@hotmail.com",
					ExpiryDateUtc = DateTime.UtcNow.AddDays(7).AddHours(6),
					Id = Guid.NewGuid().ToString(),
				},
			};
			var webapiUsers = new List<UserModel>
			{
				new UserModel
				{
					SubjectId = identityUsers[0].Id,
					Phone = "5555555550",
					Email = identityUsers[0].Email,
				},
				new UserModel
				{
					SubjectId = identityUsers[1].Id,
					Phone = "5555555551",
					Email = identityUsers[1].Email,
				},
				new UserModel
				{
					SubjectId = identityUsers[2].Id,
					Phone = "5555555552",
					Email = identityUsers[2].Email,
				},
				new UserModel
				{
					SubjectId = identityUsers[3].Id,
					Phone = "5555555553",
					Email = identityUsers[3].Email,
				},
			};
			var notificationFacade = Substitute.For<INotificationFacade>();
			var oauthClient = Substitute.For<IOAuthClient>();
			var webApiClient = Substitute.For<IWebApiClient>();
			var configuration = new ApplicationConfiguration()
			{
				RemainderDays = 7,
			};
			oauthClient.GetUsersAsync().Returns(identityUsers);
			webApiClient.GetUsersAsync().Returns(webapiUsers);
			var hostingEnvironment = Substitute.For<IHostingEnvironment>();
			hostingEnvironment.EnvironmentName.Returns("prod");
			var task = new SendLicenseExpirationNotice(
				notificationFacade,
				oauthClient,
				webApiClient,
				configuration,
				hostingEnvironment,
				null);
			await task.NotifyUsersAsync();
			await notificationFacade.Received().SendEmailAsync(
				Arg.Any<UserModel>(), Arg.Any<Template>(), webapiUsers[1].Email);
			await notificationFacade.Received().SendEmailAsync(
				Arg.Any<UserModel>(), Arg.Any<Template>(), webapiUsers[3].Email);
			await notificationFacade.Received().SendSmsAsync(
				Arg.Any<UserModel>(), Arg.Any<Template>(), webapiUsers[1].Phone);
			await notificationFacade.Received().SendSmsAsync(
				Arg.Any<UserModel>(), Arg.Any<Template>(), webapiUsers[3].Phone);
		}
	}
}
