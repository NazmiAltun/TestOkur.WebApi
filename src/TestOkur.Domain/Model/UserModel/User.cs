namespace TestOkur.Domain.Model.UserModel
{
    using System.Collections.Generic;
    using TestOkur.Domain.Model.CityModel;
    using TestOkur.Domain.SeedWork;

    public class User : Entity, IAuditable
    {
        public User(
            string subjectId,
            City city,
            District district,
            Email email,
            Phone phone,
            Name firstName,
            Name lastName,
            Name schoolName,
            Name registrarFullName,
            Phone registrarPhone,
            string referrer,
            string notes)
        : this()
        {
            SubjectId = subjectId;
            City = city;
            District = district;
            Email = email;
            Phone = phone;
            FirstName = firstName;
            LastName = lastName;
            SchoolName = schoolName;
            RegistrarFullName = registrarFullName;
            RegistrarPhone = registrarPhone;
            Referrer = referrer;
            Notes = notes;
        }

        protected User()
        {
            SmsBalance = 0;
        }

        public string SubjectId { get; private set; }

        public int SmsBalance { get; private set; }

        public City City { get; private set; }

        public District District { get; private set; }

        public Email Email { get; private set; }

        public Phone Phone { get; private set; }

        public Name FirstName { get; private set; }

        public Name LastName { get; private set; }

        public Name SchoolName { get; private set; }

        public Name RegistrarFullName { get; private set; }

        public Phone RegistrarPhone { get; private set; }

        public string Referrer { get; private set; }

        public string Notes { get; private set; }

        public void AddSmsBalance(int amount)
        {
            if (amount <= 0)
            {
                throw DomainException.With("Invalid addition amount :{0}", amount);
            }

            SmsBalance += amount;
        }

        public void DeductSmsBalance(int amount)
        {
            if (amount <= 0)
            {
                throw DomainException.With("Invalid deduction amount :{0}", amount);
            }

            if (SmsBalance - amount < 0)
            {
                throw DomainException.With(
                    "Insufficient SMS credit.Current credit :{0}, DeductAmount :{1} ",
                    SmsBalance,
                    amount);
            }

            SmsBalance -= amount;
        }

        public void Update(City city, District district, Name schoolName, Phone phone)
        {
            City = city;
            District = district;
            Phone = phone;
            SchoolName = schoolName;
        }

        public void Update(
            Email email,
            Name firstName,
            Name lastName,
            City city,
            District district,
            Name schoolName,
            Phone phone,
            string referrer,
            string notes)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Notes = notes;
            Referrer = referrer;
            Update(city, district, schoolName, phone);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
