namespace TestOkur.Domain.SeedWork
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public abstract class Enumeration : Entity, IComparable
    {
        protected Enumeration()
        {
        }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; private set; }

        public static IEnumerable<T> GetAll<T>()
            where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = Activator.CreateInstance(typeof(T), true);
                var locatedValue = info.GetValue(instance) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

        public override string ToString() => Name;

        public override int GetHashCode() => Id.GetHashCode();

        public int CompareTo(object obj) => Id.CompareTo(((Enumeration)obj).Id);
    }
}
