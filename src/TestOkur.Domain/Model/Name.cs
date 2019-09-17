namespace TestOkur.Domain.Model
{
    using System;
    using TestOkur.Common;
    using TestOkur.Domain.SeedWork;

    public class Name : ValueObject
	{
		protected Name()
		{
		}

		private Name(string value)
		{
			Validate(value);

			Value = value;
		}

		public string Value { get; private set; }

		public static implicit operator Name(string value)
		{
			return new Name(value);
		}

		public static bool operator ==(Name name1, Name name2) =>
			string.Equals(name1?.Value, name2?.Value, StringComparison.InvariantCultureIgnoreCase);

		public static bool operator !=(Name name1, Name name2) =>
			!string.Equals(name1?.Value, name2?.Value, StringComparison.InvariantCultureIgnoreCase);

		public static void Validate(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException(nameof(value), ErrorCodes.NameCannotBeEmpty);
			}

			if (value.Length > 150)
			{
				throw new ArgumentException($"{value} is too long", nameof(value));
			}
		}

		public override bool Equals(object obj) => Value.Equals((string)obj);

		public override int GetHashCode() => Value.GetHashCode();
	}
}
