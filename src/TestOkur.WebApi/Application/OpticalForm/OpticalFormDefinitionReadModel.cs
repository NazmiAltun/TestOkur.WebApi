namespace TestOkur.WebApi.Application.OpticalForm
{
	using System.Collections.Generic;

	public class OpticalFormDefinitionReadModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int StudentNoFillWidth { get; set; }

        public int StudentNoXInterval { get; set; }

        public int StudentNoYInterval { get; set; }

        public bool HasBoxForStudentNumber { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public int TextDirection { get; set; }

        public int StudentNumberFillDirection { get; set; }

        public int SchoolType { get; set; }

        public int OpticalFormTypeId { get; set; }

        public List<OpticalFormTextLocationReadModel> TextLocations { get; set; } = new List<OpticalFormTextLocationReadModel>();
    }
}
