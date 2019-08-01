namespace TestOkur.Report.Extensions
{
	using System;
	using MassTransit;
	using TestOkur.Report.Consumers;

	public static class DependencyInjectionReceiveEndpointExtensions
	{
		public static void RegisterConsumers(this IReceiveEndpointConfigurator configurator, IServiceProvider provider)
		{
			configurator.Consumer<EvaluateExamFaultConsumer>(provider);
			configurator.Consumer<ExamDeletedConsumer>(provider);
			configurator.Consumer<ExamCreatedConsumer>(provider);
			configurator.Consumer<ExamUpdatedConsumer>(provider);
			configurator.Consumer<LessonNameChangedConsumer>(provider);
			configurator.Consumer<SubjectChangedConsumer>(provider);
			configurator.Consumer<ClassroomDeletedConsumer>(provider);
			configurator.Consumer<ClassroomUpdatedConsumer>(provider);
			configurator.Consumer<StudentDeletedConsumer>(provider);
			configurator.Consumer<StudentUpdatedConsumer>(provider);
			configurator.Consumer<EvaluateExamConsumer>(provider);
		}
	}
}
