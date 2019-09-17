namespace TestOkur.WebApi.Application.Scan
{
    using System;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.Cqrs;

    [DataContract]
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

		[DataMember]
		public int ExamId { get; private set; }

		[DataMember]
		public bool ByCamera { get; private set; }

		[DataMember]
		public bool ByFile { get; private set; }

		[DataMember]
		public string Source { get; private set; }
	}
}
