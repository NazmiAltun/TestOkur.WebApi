namespace TestOkur.Report.Domain.Statistics
{
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Extensions;

    public static class StatisticsCalculator
    {
        public static ExamStatistics Calculate(IReadOnlyCollection<StudentOpticalForm> forms)
        {
            if (forms == null || forms.Count == 0)
            {
                return ExamStatistics.Empty;
            }

            var cityAttendanceCounts = new CountTable();
            var districtAttendanceCounts = new CountTable();
            var schoolAttendanceCounts = new CountTable();
            var classroomAttendanceCounts = new CountTable();
            var cityScoreSums = new SumTable<StudentOpticalForm, int>(x => x.CityId, x => x.Score);
            var districtScoreSums = new SumTable<StudentOpticalForm, int>(x => x.DistrictId, x => x.Score);
            var schoolScoreSums = new SumTable<StudentOpticalForm, int>(x => x.SchoolId, x => x.Score);
            var classroomScoreSums = new SumTable<StudentOpticalForm, int>(x => x.ClassroomId, x => x.Score);
            var scoreSum = 0f;
            var generalSuccessPercentSums = new SumTable<StudentOpticalFormSection, string>(x => x.LessonName, x => x.SuccessPercent);
            var generalNetSums = new SumTable<StudentOpticalFormSection, string>(x => x.LessonName, x => x.Net);
            var cityNetSums = new GroupedSumTable(x => x.CityId, y => y.Net);
            var citySuccessPercentSums = new GroupedSumTable(x => x.CityId, y => y.SuccessPercent);
            var districtNetSums = new GroupedSumTable(x => x.DistrictId, y => y.Net);
            var districtSuccessPercentSums = new GroupedSumTable(x => x.DistrictId, y => y.SuccessPercent);
            var schoolNetSums = new GroupedSumTable(x => x.SchoolId, y => y.Net);
            var schoolSuccessPercentSums = new GroupedSumTable(x => x.SchoolId, y => y.SuccessPercent);
            var classroomNetSums = new GroupedSumTable(x => x.ClassroomId, y => y.Net);
            var classroomSuccessPercentSums = new GroupedSumTable(x => x.ClassroomId, y => y.SuccessPercent);
            var schoolCorrectSums = new GroupedSumTable(x => x.SchoolId, y => y.CorrectCount);
            var schoolEmptySums = new GroupedSumTable(x => x.SchoolId, y => y.EmptyCount);
            var schoolWrongSums = new GroupedSumTable(x => x.SchoolId, y => y.WrongCount);
            var lessonNameIdMap = new Dictionary<string, int>();

            foreach (var form in forms)
            {
                cityAttendanceCounts.Increment(form.CityId);
                districtAttendanceCounts.Increment(form.DistrictId);
                schoolAttendanceCounts.Increment(form.SchoolId);
                classroomAttendanceCounts.Increment(form.ClassroomId);
                cityScoreSums.Add(form);
                districtScoreSums.Add(form);
                schoolScoreSums.Add(form);
                classroomScoreSums.Add(form);
                scoreSum += form.Score;

                foreach (var section in form.Sections)
                {
                    lessonNameIdMap.TryAdd(section.LessonName, section.LessonName.GetDeterministicHashCode());
                    generalSuccessPercentSums.Add(section);
                    generalNetSums.Add(section);
                    cityNetSums.Add(form, section);
                    citySuccessPercentSums.Add(form, section);
                    districtNetSums.Add(form, section);
                    districtSuccessPercentSums.Add(form, section);
                    schoolNetSums.Add(form, section);
                    schoolCorrectSums.Add(form, section);
                    schoolEmptySums.Add(form, section);
                    schoolWrongSums.Add(form, section);
                    schoolSuccessPercentSums.Add(form, section);
                    classroomNetSums.Add(form, section);
                    classroomSuccessPercentSums.Add(form, section);
                }
            }

            var sectionAverages = new Dictionary<string, SectionAverage>();

            foreach (var lessonName in generalNetSums.Keys)
            {
                var sectionAverage = new SectionAverage()
                {
                    GeneralNet = generalNetSums.GetAverage(lessonName),
                    GeneralSuccessPercent = generalSuccessPercentSums.GetAverage(lessonName),
                    CityNets = cityNetSums.GetAverage(lessonName),
                    CitySuccessPercents = citySuccessPercentSums.GetAverage(lessonName),
                    DistrictNets = districtNetSums.GetAverage(lessonName),
                    DistrictSuccessPercents = districtSuccessPercentSums.GetAverage(lessonName),
                    SchoolNets = schoolNetSums.GetAverage(lessonName),
                    SchoolCorrectCounts = schoolCorrectSums.GetAverage(lessonName),
                    SchoolWrongCounts = schoolWrongSums.GetAverage(lessonName),
                    SchoolEmptyCounts = schoolEmptySums.GetAverage(lessonName),
                    SchoolSuccessPercents = schoolSuccessPercentSums.GetAverage(lessonName),
                    ClassroomNets = classroomNetSums.GetAverage(lessonName),
                    ClassroomSuccessPercents = classroomSuccessPercentSums.GetAverage(lessonName),
                };
                sectionAverages.Add(lessonNameIdMap[lessonName].ToString(), sectionAverage);
            }

            return new ExamStatistics
            {
                ExamId = forms.First().ExamId,
                GeneralAttendanceCount = forms.Count,
                CityAttendanceCounts = cityAttendanceCounts.Dictionary,
                DistrictAttendanceCounts = districtAttendanceCounts.Dictionary,
                SchoolAttendanceCounts = schoolAttendanceCounts.Dictionary,
                ClassroomAttendanceCounts = classroomAttendanceCounts.Dictionary,
                AverageScore = scoreSum / forms.Count,
                CityAverageScores = cityScoreSums.ToAverageDictionary(),
                DistrictAverageScores = districtScoreSums.ToAverageDictionary(),
                SchoolAverageScores = schoolScoreSums.ToAverageDictionary(),
                ClassroomAverageScores = classroomScoreSums.ToAverageDictionary(),
                SectionAverages = sectionAverages,
                CreatedOnUtc = DateTime.UtcNow,
            };
        }
    }
}
