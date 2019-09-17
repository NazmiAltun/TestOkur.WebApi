namespace TestOkur.Domain.Model.ExamModel
{
    using System;
    using TestOkur.Domain.SeedWork;

    public class ExamScanSession : Entity, IAuditable
	{
		public ExamScanSession(
			Exam exam,
			Guid reportId,
			bool byCamera,
			bool byFile,
			string source)
		{
			Exam = exam;
			ReportId = reportId;
			ByCamera = byCamera;
			ByFile = byFile;
			Source = source;
		}

		protected ExamScanSession()
		{
		}

		public Exam Exam { get; private set; }

		public Guid ReportId { get; private set; }

		public bool ByCamera { get; private set; }

		public bool ByFile { get; private set; }

		public string Source { get; private set; }

		public DateTime StartDateTimeUtc { get; private set; }

		public DateTime EndDateTimeUtc { get; private set; }

		public int ScannedStudentCount { get; private set; }

		public void Start()
		{
			StartDateTimeUtc = DateTime.UtcNow;
		}

		public void End(int scannedStudentCount)
		{
			ScannedStudentCount = scannedStudentCount;
			EndDateTimeUtc = DateTime.UtcNow;
		}
	}
}
