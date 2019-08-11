namespace TestOkur.Report.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Form;

	public class StudentOrderList
	{
		private readonly string _orderName;
		private readonly Func<StudentOpticalForm, float> _selector;
		private readonly List<float> _generalOrderList;
		private readonly Dictionary<int, List<float>> _districtOrderList;
		private readonly Dictionary<int, List<float>> _classroomOrderList;
		private readonly Dictionary<int, List<float>> _schoolOrderList;
		private readonly Dictionary<int, List<float>> _cityOrderList;

		public StudentOrderList(
			string orderName,
			IReadOnlyCollection<StudentOpticalForm> forms,
			Func<StudentOpticalForm, float> selector)
		{
			_selector = selector;
			_orderName = orderName;
			_districtOrderList = CreateList(forms, f => f.DistrictId);
			_classroomOrderList = CreateList(forms, f => f.ClassroomId);
			_schoolOrderList = CreateList(forms, f => f.SchoolId);
			_cityOrderList = CreateList(forms, f => f.CityId);
			_generalOrderList = CreateList(forms, f => default).First().Value;
		}

		public StudentOrder GetStudentOrder(StudentOpticalForm form)
		{
			return new StudentOrder(
				_orderName,
				_classroomOrderList[form.ClassroomId].IndexOf(_selector(form)) + 1,
				_schoolOrderList[form.SchoolId].IndexOf(_selector(form)) + 1,
				_districtOrderList[form.DistrictId].IndexOf(_selector(form)) + 1,
				_cityOrderList[form.CityId].IndexOf(_selector(form)) + 1,
				_generalOrderList.IndexOf(_selector(form)) + 1);
		}

		private Dictionary<int, List<float>> CreateList(
			IEnumerable<StudentOpticalForm> forms,
			Func<StudentOpticalForm, int> groupByFunc)
		{
			return forms
				.GroupBy(groupByFunc)
				.ToDictionary(g => g.Key, g =>
					g.Select(_selector)
						.OrderByDescending(x => x)
						.Distinct()
						.ToList());
		}
	}
}
