namespace TestOkur.Report.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SchoolOrderList
    {
        private readonly Func<SchoolResult, float> _selector;
        private readonly List<float> _generalOrderList;
        private readonly Dictionary<int, List<float>> _districtOrderList;
        private readonly Dictionary<int, List<float>> _cityOrderList;

        public SchoolOrderList(IEnumerable<SchoolResult> results, Func<SchoolResult, float> selector)
        {
            _selector = selector;
            _districtOrderList = CreateList(results, r => r.DistrictId);
            _cityOrderList = CreateList(results, r => r.CityId);
            _generalOrderList = CreateList(results, r => default).First().Value;
        }

        public int GetDistrictOrder(SchoolResult result) => _districtOrderList[result.DistrictId].IndexOf(_selector(result)) + 1;

        public int GetCityOrder(SchoolResult result) => _cityOrderList[result.CityId].IndexOf(_selector(result)) + 1;

        public int GetGeneralOrder(SchoolResult result) => _generalOrderList.IndexOf(_selector(result)) + 1;

        private Dictionary<int, List<float>> CreateList(
            IEnumerable<SchoolResult> results,
            Func<SchoolResult, int> groupByFunc)
        {
            return results
                .GroupBy(groupByFunc)
                .ToDictionary(g => g.Key, g =>
                    g.Select(_selector)
                        .OrderByDescending(x => x)
                        .Distinct()
                        .ToList());
        }
    }
}
