namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class DeleteExamCommand : CommandBase, IClearCacheWithRegion
    {
        public DeleteExamCommand(int examId, bool shared)
        {
            ExamId = examId;
            Shared = shared;
        }

        public string Region => Shared ? "Exams" : string.Empty;

        public IEnumerable<string> CacheKeys => new[] { $"Exams_{UserId}" };

        public int ExamId { get; private set; }

        public bool Shared { get; private set; }
    }
}
