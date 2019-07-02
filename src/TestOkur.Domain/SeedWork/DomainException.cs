namespace TestOkur.Domain.SeedWork
{
	using System;

	public class DomainException : Exception
    {
        protected DomainException(string message)
            : base(message)
        {
        }

        protected DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public static DomainException With(string format, params object[] args)
        {
            var message = args.Length <= 0
                ? format
                : string.Format(format, args);
            return new DomainException(message);
        }

        public static DomainException With(Exception innerException, string format, params object[] args)
        {
            var message = args.Length <= 0
                ? format
                : string.Format(format, args);
            return new DomainException(message, innerException);
        }
    }
}
