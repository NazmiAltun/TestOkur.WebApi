namespace TestOkur.Report.Domain.Optic
{
    public static class Answers
    {
        public static bool IsEmpty(byte answer)
        {
            return answer == ' ' || answer == '\0';
        }

        public static bool IsValid(byte answer)
        {
            return answer == ' ' || answer == '\0' || answer == 'A' || answer == 'B' || answer == 'C' ||
                   answer == 'D' || answer == 'E';
        }
    }
}
