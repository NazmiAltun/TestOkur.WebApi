namespace TestOkur.Domain.Model
{
	using System;
	using TestOkur.Domain.SeedWork;

	public class StudentNumber : ValueObject
	{
		public const int Min = 0;
		public const int Max = 99999;

		protected StudentNumber()
		{
		}

		private StudentNumber(int value)
		{
			if (!IsValid(value))
			{
				throw new ArgumentOutOfRangeException(
					nameof(value),
					$"Student number must be in range of ({Min},{Max}]");
			}

			Value = value;
		}

		public int Value { get; private set; }

		public static implicit operator StudentNumber(int value)
		{
			return new StudentNumber(value);
		}

		public static bool IsValid(int value)
		{
			return value > Min && value <= Max;
		}
	}
}
