namespace TestOkur.Domain.SeedWork
{
    using System.Collections.Generic;

    public abstract class Entity : ValueObject
    {
        public long Id { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
