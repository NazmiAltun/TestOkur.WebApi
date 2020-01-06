namespace TestOkur.WebApi.Application.Contact
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Domain.Model.StudentModel;
    using TestOkur.Infrastructure.CommandsQueries;

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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public int ContactType { get; set; }

        public string Labels { get; set; }

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
