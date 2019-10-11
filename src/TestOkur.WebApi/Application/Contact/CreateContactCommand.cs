namespace TestOkur.WebApi.Application.Contact
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using TestOkur.Domain.Model.StudentModel;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
    public class CreateContactCommand : CommandBase, IClearCache
    {
        public CreateContactCommand(Guid id, string firstName, string lastName, string phone, int contactType)
            : this(id, firstName, lastName, phone, contactType, null)
        {
        }

        public CreateContactCommand(Guid id, string firstName, string lastName, string phone, int contactType, string labels)
        : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            ContactType = contactType;
            Labels = labels;
        }

        public CreateContactCommand()
        {
        }

        [DataMember]
        public string FirstName { get; private set; }

        [DataMember]
        public string LastName { get; private set; }

        [DataMember]
        public string Phone { get; private set; }

        [DataMember]
        public int ContactType { get; private set; }

        [DataMember]
        public string Labels { get; private set; }

        public IEnumerable<string> CacheKeys => new[] { $"Contacts_{UserId}" };

        public Contact ToDomainModel()
        {
            try
            {
                return new Contact(
                    FirstName,
                    LastName,
                    Phone,
                    Domain.SeedWork.Enumeration.GetAll<ContactType>()
                        .First(t => t.Id == ContactType),
                    Labels);
            }
            catch
            {
                return null;
            }
        }
    }
}
