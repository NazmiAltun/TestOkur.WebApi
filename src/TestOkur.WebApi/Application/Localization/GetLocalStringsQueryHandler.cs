namespace TestOkur.WebApi.Application.Localization
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using OfficeOpenXml;
    using Paramore.Darker;
    using Paramore.Darker.QueryLogging;
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetLocalStringsQueryHandler : QueryHandler<GetLocalStringsQuery, IReadOnlyCollection<LocalString>>
    {
        [QueryLogging(1)]
        [ResultCaching(2)]
        public override IReadOnlyCollection<LocalString> Execute(GetLocalStringsQuery query)
        {
            return ReadFromExcel(query.CultureCode);
        }

        private IReadOnlyCollection<LocalString> ReadFromExcel(string cultureCode)
        {
            var dict = new Dictionary<string, LocalString>();

            using (var package = GetExcelPackage($"{cultureCode}.xlsx"))
            {
                var workSheet = package.Workbook.Worksheets.First();

                for (var i = 2; i < workSheet.Dimension.Rows + 1; i++)
                {
                    var localString = ParseLocalString(workSheet, i);
                    dict.TryAdd(localString.Name, localString);
                }
            }

            return dict.Values.ToList();
        }

        private LocalString ParseLocalString(ExcelWorksheet workSheet, int rowIndex)
        {
            return new LocalString(
                workSheet.Cells[rowIndex, 1].Value.ToString(),
                workSheet.Cells[rowIndex, 2].Value.ToString());
        }

        private ExcelPackage GetExcelPackage(string filePath)
        {
            var file = new FileInfo(Path.Combine("Application", "Localization", filePath));

            return file.Exists ? new ExcelPackage(file) :
                throw new FileNotFoundException("Not found", filePath);
        }
    }
}
