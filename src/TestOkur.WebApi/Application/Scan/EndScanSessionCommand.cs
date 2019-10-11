namespace TestOkur.WebApi.Application.Scan
{
    using System;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
    public class EndScanSessionCommand : CommandBase
    {
        public EndScanSessionCommand(Guid id, int scannedStudentCount)
            : base(id)
        {
            ScannedStudentCount = scannedStudentCount;
        }

        [DataMember]
        public int ScannedStudentCount { get; private set; }
    }
}
