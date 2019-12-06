namespace TestOkur.Domain.Model
{
    using System;
    using TestOkur.Domain.SeedWork;

    public class Grade : ValueObject
    {
        public const int Min = 1;
        public const int Max = 12;

        protected Grade()
        {
        }

        private Grade(int value)
        {
            if (!CheckIfValid(value))
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

        public static bool CheckIfValid(int value) => value >= Min && value <= Max;

        public static bool CheckIfHighSchool(int value) => value > 8;

        public static bool CheckIfSecondarySchool(int value) => value < 9;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
