namespace TestOkur.WebApi.Application.Scan
{
    using System;
    using TestOkur.Infrastructure.CommandsQueries;

    public class StartScanSessionCommand : CommandBase
    {
        public StartScanSessionCommand(
            Guid id,
            int examId,
            bool byCamera,
            bool byFile,
            string source)
            : base(id)
        {
            ExamId = examId;
            ByCamera = byCamera;
            ByFile = byFile;
            Source = source;
        }

        public StartScanSessionCommand()
        {
        }

        public int ExamId { get; set; }

        public bool ByCamera { get; set; }

        public bool ByFile { get; set; }

        public string Source { get; set; }
    }
}
