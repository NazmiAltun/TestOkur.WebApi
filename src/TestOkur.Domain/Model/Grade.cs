namespace TestOkur.Domain.Model
{
	using System;
	using System.Collections.Generic;
	using TestOkur.Domain.SeedWork;

	public class Grade : ValueObject
	{
		public const int Min = 1;
		public const int Max = 12;

		public static readonly IEnumerable<Grade> HighSchool = new Grade[] { 9, 10, 11, 12 };
		public static readonly IEnumerable<Grade> SecondarySchool = new Grade[] { 5, 6, 7, 8 };

		protected Grade()
		{
		}

		private Grade(int value)
		{
			if (!IsValid(value))
			{
				throw new ArgumentOutOfRangeException(nameof(value));
			}

			Value = value;
		}

		public int Value { get; private set; }

		public static implicit operator Grade(int value)
		{
			return new Grade(value);
		}

		public static implicit operator int(Grade grade)
		{
			return grade.Value;
		}

		public static bool operator <(Grade x, Grade y)
		{
			return x.Value < y.Value;
		}

		public static bool operator >(Grade x, Grade y)
		{
			return x.Value > y.Value;
		}

		public static bool IsValid(int value) => value >= Min && value <= Max;

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
