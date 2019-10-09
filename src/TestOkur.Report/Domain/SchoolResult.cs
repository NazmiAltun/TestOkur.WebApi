﻿namespace TestOkur.Report.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Optic.Form;

    public class SchoolResult
    {
        public SchoolResult(IEnumerable<StudentOpticalForm> forms)
        {
            var form = forms.First();
            ExamId = form.ExamId;
            SchoolId = form.SchoolId;
            SchoolName = form.SchoolName;
            CityId = form.CityId;
            CityName = form.CityName;
            DistrictId = form.DistrictId;
            DistrictName = form.DistrictName;
            ScoreAverage = form.SchoolScoreAverage;
            StudentCount = form.SchoolAttendanceCount;
            CreatedOnUtc = DateTime.UtcNow;
            ClassroomCount = forms.Select(f => f.ClassroomId).Distinct().Count();
            SuccessPercent = forms.Average(f => f.SuccessPercent);

            var groupedSections = forms.SelectMany(f => f.Sections)
                .GroupBy(s => s.LessonName);

            Sections = form.Sections
                .Select(s => new SchoolResultSection()
                {
                    LessonName = s.LessonName,
                    CityAverageNet = s.CityAverageNet,
                    CorrectCount = groupedSections
                        .First(g => g.Key == s.LessonName)
                        .Average(x => (float)x.CorrectCount),
                    DistrictAverageNet = s.DistrictAverageNet,
                    EmptyCount = groupedSections
                        .First(g => g.Key == s.LessonName)
                        .Average(x => (float)x.EmptyCount),
                    GeneralAverageNet = s.GeneralAverageNet,
                    Net = s.SchoolAverageNet,
                    WrongCount = groupedSections
                        .First(g => g.Key == s.LessonName)
                        .Average(x => (float)x.WrongCount),
                    QuestionCount = s.QuestionCount,
                    SuccessPercent = groupedSections
                        .First(g => g.Key == s.LessonName)
                        .Average(x => x.SuccessPercent),
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