namespace TestOkur.Notification.Dtos
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
    public enum SmsStatus
    {
        Pending,
        Successful,
        TryingAgain,
        Failed,
    }
}
