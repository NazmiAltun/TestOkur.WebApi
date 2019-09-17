﻿namespace TestOkur.WebApi.Application.Localization
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using OfficeOpenXml;
    using Paramore.Darker;
    using Paramore.Darker.QueryLogging;
    using TestOkur.Infrastructure.Cqrs;

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
            var list = new List<LocalString>();

            using (var package = GetExcelPackage($"{cultureCode}.xlsx"))
            {
                var workSheet = package.Workbook.Worksheets.First();

                for (var i = 2; i < workSheet.Dimension.Rows + 1; i++)
                {
                    list.Add(ParseLocalString(workSheet, i));
                }
            }

            return list;
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
