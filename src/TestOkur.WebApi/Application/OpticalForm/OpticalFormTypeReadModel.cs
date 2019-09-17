namespace TestOkur.WebApi.Application.OpticalForm
{
    using System.Collections.Generic;

    public class OpticalFormTypeReadModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Configuration { get; set; }

        public string ConfigurationFile { get; set; }

        public int MaxQuestionCount { get; set; }

        public int SchoolType { get; set; }

        public List<OpticalFormDefinitionReadModel> OpticalFormDefinitions { get; set; }
            = new List<OpticalFormDefinitionReadModel>();

        public List<FormLessonSectionReadModel> FormLessonSections { get; set; }
            = new List<FormLessonSectionReadModel>();
    }
}
