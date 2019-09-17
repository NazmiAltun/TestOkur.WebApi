namespace TestOkur.Domain.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using TestOkur.Common;
    using TestOkur.Domain.SeedWork;

    public class Email : ValueObject
    {
        private static readonly EmailAddressAttribute EmailAddressAttribute = new EmailAddressAttribute();

        protected Email()
        {
        }

        private Email(string value)
        {
            if (string.IsNullOrEmpty(value) || !IsValid(value))
            {
                throw new ArgumentException(ErrorCodes.InvalidEmailAddress, nameof(value));
            }

            Value = value;
        }

        public string Value { get; private set; }

        public static implicit operator Email(string value) => new Email(value);

        public static implicit operator string(Email value) => value.Value;

        public static bool IsValid(string value) =>
            EmailAddressAttribute.IsValid(value);

        public override string ToString() => Value;
    }
}
