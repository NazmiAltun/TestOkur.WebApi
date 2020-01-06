namespace TestOkur.WebApi.Application.Contact
{
    using System;

    public class EditContactCommand : CreateContactCommand
    {
        public EditContactCommand(Guid id, string firstName, string lastName, string phone, int contactType, string labels, int contactId)
            : base(id, firstName, lastName, phone, contactType, labels)
        {
            ContactId = contactId;
        }

        public EditContactCommand()
        {
        }

        public int ContactId { get; set; }
    }
}
