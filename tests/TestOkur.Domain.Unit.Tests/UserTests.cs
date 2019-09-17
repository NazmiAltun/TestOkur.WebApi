namespace TestOkur.WebApi.Unit.Tests.Domain
{
    using System;
    using FluentAssertions;
    using TestOkur.Domain.Model.CityModel;
    using TestOkur.Domain.Model.UserModel;
    using TestOkur.Domain.SeedWork;
    using Xunit;

    public class UserTests
	{
		private readonly User _user;

		public UserTests()
		{
			_user = new User(
				Guid.NewGuid().ToString(),
				new City(34, "Istanbul"),
				new District(450, "Pendik"),
				"nazmi@testokur.com",
				"5544205163",
				"Nazmi",
				"Altun",
				"OnDokuz Mayis",
				"Nazmi Bora Altun",
				"5324987788",
				"RandomNotes");
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-4)]
		public void AddSmsCredits_Throws_WhenAmountIsInvalid(int amount)
		{
			Action act = () => _user.AddSmsBalance(amount);
			act.Should().Throw<DomainException>();
		}

		[Fact]
		public void AddSmsCredits_Should_AddAmount()
		{
			var credit = _user.SmsBalance;
			const int additionAmount = 5;
			_user.AddSmsBalance(additionAmount);
			_user.SmsBalance.Should().Be(credit + additionAmount);
		}

		[Fact]
		public void DeductSmsCredit_Throws_WhenDeductionAmountIsBiggerThanBalance()
		{
			var credit = _user.SmsBalance;
			const int additionAmount = 5;
			_user.AddSmsBalance(additionAmount);
			Action act = () => _user.DeductSmsBalance(credit + (2 * additionAmount));
			act.Should().Throw<DomainException>();
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		public void DeductSmsCredit_Throws_WhenDeductionAmountIsInvalid(int amount)
		{
			Action act = () => _user.DeductSmsBalance(amount);
			act.Should().Throw<DomainException>();
		}

		[Fact]
		public void DeductSmsCredit_Deducts_WhenDeductionAmountIsValidAndLessThanBalance()
		{
			const int additionAmount = 5;
			const int deductionAmount = 3;
			_user.AddSmsBalance(additionAmount);
			var credit = _user.SmsBalance;
			_user.DeductSmsBalance(deductionAmount);
			_user.SmsBalance.Should().Be(credit - deductionAmount);
		}

		[Fact]
		public void Update_UpdatesUser()
		{
			_user.Update(
				new City(20, "Denizli"),
				new District(500, "Pamukkale"),
				"TestSchool",
				"5324256878");
			_user.City.Id.Should().Be(20);
			_user.District.Id.Should().Be(500);
			_user.SchoolName.Value.Should().Be("TestSchool");
		}
	}
}
