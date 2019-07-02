namespace TestOkur.Notification.Unit.Tests.Extensions
{
	using FluentAssertions;
	using TestOkur.Notification.Extensions;
	using Xunit;

	public class ToSmsFriendlyShould
	{
		[Theory]
		[InlineData("öçşğüÇŞĞÜÖİı", "ocsguCSGUOIi")]
		[InlineData(null, null)]
		[InlineData("", "")]
		public void ReplaceTurkishCharacters(string value, string expected)
		{
			value.ToSmsFriendly().Should().Be(expected);
		}
	}
}
