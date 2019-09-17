namespace TestOkur.Domain.Model.SmsModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SmsCreditCalculator : ISmsCreditCalculator
    {
        private const decimal CharacterCountPerSms = 160;

        public int Calculate(string message)
        {
            return 1 + (int)Math.Floor(message.Length / CharacterCountPerSms);
        }

        public int Calculate(IEnumerable<string> messages)
        {
            return messages.Sum(Calculate);
        }
    }
}
