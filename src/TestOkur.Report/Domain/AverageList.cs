namespace TestOkur.Report.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Optic.Form;
    using static System.Math;

    public class AverageList
    {
        private readonly string _name;
        private readonly Func<StudentOpticalFormSection, float> _averageSelector;
        private readonly Dictionary<string, float> _general;
        private readonly Dictionary<int, Dictionary<string, float>> _district;
        private readonly Dictionary<int, Dictionary<string, float>> _city;
        private readonly Dictionary<int, Dictionary<string, float>> _school;
        private readonly Dictionary<int, Dictionary<string, float>> _classroom;

        public AverageList(
            string name,
            IReadOnlyCollection<StudentOpticalForm> forms,
            Func<StudentOpticalFormSection, float> averageSelector)
        {
            _averageSelector = averageSelector;
            _name = name;
            _general = Calculate(forms, f => default).First().Value;
            _district = Calculate(forms, f => f.DistrictId);
            _school = Calculate(forms, f => f.SchoolId);
            _classroom = Calculate(forms, f => f.ClassroomId);
            _city = Calculate(forms, f => f.CityId);
        }

        public Average Get(StudentOpticalForm form, string lessonName)
        {
            return new Average(
                _name,
                GetGeneralAverage(lessonName),
                GetCityAverage(lessonName, form.CityId),
                GetDistrictAverage(lessonName, form.DistrictId),
                GetSchoolAverage(lessonName, form.SchoolId),
                GetClassroomAverage(lessonName, form.ClassroomId));
        }

        public float GetGeneralAverage(string lessonName)
        {
            return _general.TryGetValue(lessonName, out var value) ? value : 0;
        }

        public float GetDistrictAverage(string lessonName, int districtId)
        {
            return _district[districtId].TryGetValue(lessonName, out var value) ? value : 0;
        }

        public float GetCityAverage(string lessonName, int cityId)
        {
            return _city[cityId].TryGetValue(lessonName, out var value) ? value : 0;
        }

        public float GetClassroomAverage(string lessonName, int classroomId)
        {
            return _classroom[classroomId].TryGetValue(lessonName, out var value) ? value : 0;
        }

        public float GetSchoolAverage(string lessonName, int userId)
        {
            return _school[userId].TryGetValue(lessonName, out var value) ? value : 0;
        }

        private Dictionary<int, Dictionary<string, float>> Calculate(
            IEnumerable<StudentOpticalForm> forms,
            Func<StudentOpticalForm, int> selector)
        {
            return forms.GroupBy(selector)
                .ToDictionary(g => g.Key, g =>
                    g.SelectMany(f => f.Sections)
                        .GroupBy(x => x.LessonName)
                        .ToDictionary(y => y.Key, y => (float)Round(y.Average(_averageSelector) * 100) / 100));
        }
    }
}
