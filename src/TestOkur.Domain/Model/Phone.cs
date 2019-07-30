namespace TestOkur.Domain.Model
{
	using System;
	using System.Linq;
	using TestOkur.Domain.SeedWork;

	public class Phone : ValueObject
    {
        protected Phone()
        {
        }

        private Phone(string value)
        {
            Validate(value);
            Value = value;
        }

        public string Value { get; private set; }

        public static implicit operator Phone(string value)
        {
            return new Phone(value);
        }

        public static implicit operator string(Phone phone)
        {
            return phone.Value;
        }

        public static void Validate(string value)
	    {
		    if (string.IsNullOrEmpty(value))
		    {
			    throw new ArgumentNullException(value);
		    }

		    if (value.Length != 10 || !value.All(char.IsDigit) || !value.StartsWith("5"))
		    {
			    throw new ArgumentException($"Invalid phone number: {value}", nameof(value));
		    }
	    }
	}
}
