namespace TestOkur.Domain.Model.OpticalFormModel
{
    using System.Collections.Generic;
    using TestOkur.Domain.SeedWork;

    public class OpticalFormDefinition : Entity
    {
        public OpticalFormDefinition(
            string name,
            int studentNoFillWidth,
            int studentNoXInterval,
            int studentNoYInterval,
            bool hasBoxForStudentNumber,
            string description,
            string path,
            Direction textDirection,
            Direction studentNumberFillDirection,
            SchoolType schoolType,
            IReadOnlyCollection<OpticalFormTextLocation> textLocations)
        {
            Name = name;
            StudentNoFillWidth = studentNoFillWidth;
            StudentNoXInterval = studentNoXInterval;
            StudentNoYInterval = studentNoYInterval;
            HasBoxForStudentNumber = hasBoxForStudentNumber;
            Description = description;
            Path = path;
            TextDirection = textDirection;
            StudentNumberFillDirection = studentNumberFillDirection;
            SchoolType = schoolType;
            TextLocations = textLocations;
        }

        protected OpticalFormDefinition()
        {
        }

        public string Name { get; private set; }

        public int StudentNoFillWidth { get; private set; }

        public int StudentNoXInterval { get; private set; }

        public int StudentNoYInterval { get; private set; }

        public bool HasBoxForStudentNumber { get; private set; }

        public string Description { get; private set; }

        public string Path { get; private set; }

        public Direction TextDirection { get; private set; }

        public Direction StudentNumberFillDirection { get; private set; }

        public SchoolType SchoolType { get; private set; }

        public IReadOnlyCollection<OpticalFormTextLocation> TextLocations { get; private set; }
    }
}
