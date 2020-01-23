namespace TestOkur.Report.Domain.Optic
{
    public class StudentSectionAnswerResult
    {
        public StudentSectionAnswerResult(int studentId)
        {
            StudentId = studentId;
        }

        private StudentSectionAnswerResult()
        {
        }

        public int StudentId { get; set; }

        public SectionAnswerResult[] SectionAnswerResults { get; set; }
    }
}