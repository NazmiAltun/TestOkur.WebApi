namespace TestOkur.WebApi.Unit.Tests.Domain
{
	using System;
	using FluentAssertions;
	using TestOkur.Domain.Model;
	using Xunit;

	public class EmailTests
	{
		[Theory]
		[InlineData("bilgi@testokur.com")]
		[InlineData("nazmi@testokur.com")]
		[InlineData("nazmi@test.com")]
		public void GivenEmail_WhenValueIsValid_ThenEmailShouldBeCreated(string value)
		{
			Action action = () =>
			{
				Email email = value;
			};
			action.Should().NotThrow();
		}

		[Theory]
		[InlineData("test")]
		[InlineData("nazmi.testokur")]
		[InlineData("")]
		[InlineData(null)]
		public void GivenEmail_WhenValueIsInvalid_ThenArgumentExceptionShouldBeThrown(string value)
		{
			Action action = () =>
			{
				Email email = value;
			};
			action.Should().Throw<ArgumentException>();
		}
	}
}
