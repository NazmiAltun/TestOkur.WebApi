namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class DeleteExamCommand : CommandBase, IClearCache
    {
        public DeleteExamCommand(int examId)
        {
            ExamId = examId;
        }

        public string Region => "Exams";

        public IEnumerable<string> CacheKeys => new[] { $"Exams_{UserId}" };

        public int ExamId { get; private set; }
    }
}
