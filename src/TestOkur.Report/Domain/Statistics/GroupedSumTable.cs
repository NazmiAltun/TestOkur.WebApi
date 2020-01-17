using System;
using System.Collections.Generic;
using System.Linq;
using TestOkur.Optic.Form;

namespace TestOkur.Report.Domain.Statistics
{
    internal class GroupedSumTable
    {
        private readonly Dictionary<string, Dictionary<int, List<float>>> _dictionary;
        private readonly Func<StudentOpticalForm, int> _secondaryKeySelector;
        private readonly Func<StudentOpticalFormSection, float> _valueSelector;

        public GroupedSumTable(Func<StudentOpticalForm, int> secondaryKeySelector, Func<StudentOpticalFormSection, float> valueSelector)
        {
            _secondaryKeySelector = secondaryKeySelector;
            _valueSelector = valueSelector;
            _dictionary = new Dictionary<string, Dictionary<int, List<float>>>();
        }

        public void Add(StudentOpticalForm form, StudentOpticalFormSection section)
        {
            if (!_dictionary.ContainsKey(section.LessonName))
            {
                _dictionary.Add(section.LessonName, new Dictionary<int, List<float>>());
            }

            var secondaryKey = _secondaryKeySelector(form);
            if (!_dictionary[section.LessonName].ContainsKey(secondaryKey))
            {
                _dictionary[section.LessonName].Add(secondaryKey, new List<float>());
            }

            _dictionary[section.LessonName][secondaryKey].Add(_valueSelector(section));
        }

        public Dictionary<int, float> GetAverage(string lessonName)
        {
            var dict = _dictionary[lessonName];

            return dict.ToDictionary(x => x.Key, x => x.Value.Average());
        }
    }
}