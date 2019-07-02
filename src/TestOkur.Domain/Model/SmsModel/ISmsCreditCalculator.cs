namespace TestOkur.Domain.Model.SmsModel
{
	using System.Collections.Generic;

	public interface ISmsCreditCalculator
    {
        int Calculate(string message);

        int Calculate(IEnumerable<string> messages);
    }
}
