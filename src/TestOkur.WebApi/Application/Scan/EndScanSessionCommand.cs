namespace TestOkur.WebApi.Application.Scan
{
    using System;
    using TestOkur.Infrastructure.CommandsQueries;

    public class EndScanSessionCommand : CommandBase
    {
        public EndScanSessionCommand(Guid id, int scannedStudentCount)
            : base(id)
        {
            ScannedStudentCount = scannedStudentCount;
        }

        public EndScanSessionCommand()
        {
        }

        public int ScannedStudentCount { get; set; }
    }
}
