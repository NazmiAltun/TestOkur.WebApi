namespace TestOkur.Domain.Model.StudentModel
{
    using TestOkur.Domain.SeedWork;

    public class Contact : Entity, IAuditable
    {
        public Contact(Name firstName, Name lastName, Phone phone, ContactType contactType, string labels)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            ContactType = contactType;
            Labels = labels;
        }

        protected Contact()
        {
        }

        public Name FirstName { get; private set; }

        public Name LastName { get; private set; }

        public Phone Phone { get; private set; }

        public ContactType ContactType { get; private set; }

        public string Labels { get; private set; }

        public void Update(string firstName, string lastName, string phone, ContactType contactType, string labels)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            ContactType = contactType;
            Labels = labels;
        }
    }
}
