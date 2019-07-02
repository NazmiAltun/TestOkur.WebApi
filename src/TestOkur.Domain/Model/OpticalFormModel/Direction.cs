namespace TestOkur.Domain.Model.OpticalFormModel
{
	using TestOkur.Domain.SeedWork;

	public class Direction : Enumeration
    {
        public static readonly Direction ToBottom = new Direction(1, "ToBottom");
        public static readonly Direction ToTop = new Direction(2, "ToTop");
        public static readonly Direction ToLeft = new Direction(3, "ToLeft");
        public static readonly Direction ToRight = new Direction(4, "ToRight");

        public Direction(int id, string name)
            : base(id, name)
        {
        }

        protected Direction()
        {
        }
    }
}
