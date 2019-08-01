namespace TestOkur.WebApi.Integration.Tests.Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using MassTransit.Testing;
	using TestOkur.Contracts.Alert;
	using TestOkur.Contracts.Classroom;
	using TestOkur.Contracts.Exam;
	using TestOkur.Contracts.Lesson;
	using TestOkur.Contracts.Sms;
	using TestOkur.Contracts.Student;
	using TestOkur.Contracts.User;

	internal class Consumer : MultiTestConsumer
	{
		public Consumer()
			: base(TimeSpan.FromSeconds(10))
		{
			Consume<ISendSmsRequestReceived>();
			Consume<INewUserRegistered>();
			Consume<IUserActivated>();
			Consume<IResetPasswordTokenGenerated>();
			Consume<IExamDeleted>();
			Consume<IExamCreated>();
			Consume<IExamUpdated>();
			Consume<ILessonNameChanged>();
			Consume<ISubjectChanged>();
			Consume<IClassroomDeleted>();
			Consume<IClassroomUpdated>();
			Consume<IStudentDeleted>();
			Consume<IStudentUpdated>();
			Consume<IUserErrorReceived>();
		}

		public static Consumer Instance { get; } = new Consumer();

		public IEnumerable<T> GetAll<T>()
			where T : class
		{
			return Received.Select<T>().Select(r => r.Context.Message);
		}

		public T GetFirst<T>()
			where T : class
		{
			return Received.Select<T>().First().Context.Message;
		}
	}
}
