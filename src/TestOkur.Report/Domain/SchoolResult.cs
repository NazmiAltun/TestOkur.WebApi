namespace TestOkur.Report.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Domain.Statistics;

    public class SchoolResult
    {
        public SchoolResult(
            ExamStatistics examStatistics,
            IEnumerable<StudentOpticalForm> forms,
            IEnumerable<StudentOpticalFormSection> sections)
        {
            var form = forms.First();
            ExamId = form.ExamId;
            SchoolId = form.SchoolId;
            SchoolName = form.SchoolName;
            CityId = form.CityId;
            CityName = form.CityName;
            DistrictId = form.DistrictId;
            DistrictName = form.DistrictName;
            ScoreAverage = examStatistics.SchoolAverageScores[form.SchoolId];
            StudentCount = examStatistics.SchoolAttendanceCounts[form.SchoolId];
            CreatedOnUtc = DateTime.UtcNow;
            ClassroomCount = forms.Select(f => f.ClassroomId).Distinct().Count();
            SuccessPercent = forms.Average(f => f.SuccessPercent);

            Sections = sections
                .Select(s => new SchoolResultSection()
                {
                    LessonName = s.LessonName,
                    CityAverageNet = examStatistics.SectionAverages[s.LessonName]
                        .CityNets[form.CityId],
                    CorrectCount = examStatistics.SectionAverages[s.LessonName]
                        .SchoolCorrectCounts[form.SchoolId],
                    DistrictAverageNet = examStatistics.SectionAverages[s.LessonName]
                        .DistrictNets[form.DistrictId],
                    EmptyCount = examStatistics.SectionAverages[s.LessonName]
                        .SchoolEmptyCounts[form.SchoolId],
                    GeneralAverageNet = examStatistics.SectionAverages[s.LessonName]
                        .GeneralNet,
                    Net = examStatistics.SectionAverages[s.LessonName]
                        .SchoolNets[form.SchoolId],
                    WrongCount = examStatistics.SectionAverages[s.LessonName]
                        .SchoolWrongCounts[form.SchoolId],
                    QuestionCount = (int)(examStatistics.SectionAverages[s.LessonName]
                        .SchoolWrongCounts[form.SchoolId] +
                        examStatistics.SectionAverages[s.LessonName]
                            .SchoolCorrectCounts[form.SchoolId] +
                        examStatistics.SectionAverages[s.LessonName]
                            .SchoolEmptyCounts[form.SchoolId]),
                    SuccessPercent = examStatistics.SectionAverages[s.LessonName]
                        .SchoolSuccessPercents[form.SchoolId],
                })
                .ToList();
        }

        public SchoolResult()
        {
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public int ExamId { get; set; }

        public int SchoolId { get; set; }

        public string SchoolName { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public int DistrictId { get; set; }

        public string DistrictName { get; set; }

        public int ClassroomCount { get; set; }

        public int StudentCount { get; set; }

        public float ScoreAverage { get; set; }

        public float SuccessPercent { get; set; }

        public int DistrictOrder { get; set; }

        public int CityOrder { get; set; }

        public int GeneralOrder { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public List<SchoolResultSection> Sections { get; set; }
    }
}