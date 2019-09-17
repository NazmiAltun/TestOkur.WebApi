namespace TestOkur.Domain.Model.ExamModel
{
    using System;
    using TestOkur.Domain.SeedWork;

    public class IncorrectEliminationRate : ValueObject
    {
        protected IncorrectEliminationRate()
        {
        }

        private IncorrectEliminationRate(int value)
        {
            if (value < 0 || value > 4)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "IncorrectElimination Rate must be in range of [0,4]");
            }

            Value = value;
        }

        public int Value { get; private set; }

        public static implicit operator IncorrectEliminationRate(int value)
        {
            return new IncorrectEliminationRate(value);
        }

        public static implicit operator int(IncorrectEliminationRate value)
        {
            return value.Value;
        }
    }
}
