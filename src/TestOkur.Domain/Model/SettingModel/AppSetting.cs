namespace TestOkur.Domain.Model.SettingModel
{
	using TestOkur.Domain.SeedWork;

	public class AppSetting : Entity, IAuditable
    {
        public AppSetting(Name name, string value, string comment)
        {
            Name = name;
            Value = value;
            Comment = comment;
        }

        protected AppSetting()
        {
        }

        public Name Name { get; private set; }

        public string Value { get; private set; }

        public string Comment { get; private set; }
    }
}
