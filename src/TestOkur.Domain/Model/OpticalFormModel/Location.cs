namespace TestOkur.Domain.Model.OpticalFormModel
{
    using System;
    using TestOkur.Domain.SeedWork;

    public class Location : ValueObject
    {
        protected Location()
        {
        }

        private Location(int x, int y)
        {
            if (x < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(x), "X cannot be smaller than 0");
            }

            if (y < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(y), "Y cannot be smaller than 0");
            }

            X = x;
            Y = y;
        }

        public static Location Empty => new Location(default, default);

        public int X { get; private set; }

        public int Y { get; private set; }

        public static Location From(int x, int y) => new Location(x, y);
    }
}
