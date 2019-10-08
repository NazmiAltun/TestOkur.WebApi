namespace TestOkur.Contracts.Exam
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Optic.Form;

    public interface IExamCreated : IIntegrationEvent
    {
        int ExamId { get; }

        int IncorrectEliminationRate { get; }

        DateTime ExamDate { get; }

        string ExamName { get; }

        string ExamTypeName { get; }

        bool Shared { get; }

        IEnumerable<AnswerKeyOpticalForm> AnswerKeyOpticalForms { get; }
    }
}
