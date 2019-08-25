namespace TestOkur.WebApi.Application.LicenseType
{
    public class LicenseTypeReadModel
    {
	    public LicenseTypeReadModel(int id, string name, int maxAllowedDeviceCount, int maxAllowedRecordCount, bool canScan)
	    {
		    Id = id;
		    Name = name;
		    MaxAllowedDeviceCount = maxAllowedDeviceCount;
		    MaxAllowedRecordCount = maxAllowedRecordCount;
		    CanScan = canScan;
	    }

	    public int Id { get; set; }

	    public string Name { get; set; }

	    public int MaxAllowedDeviceCount { get; set; }

	    public int MaxAllowedRecordCount { get; set; }

	    public bool CanScan { get; set; }
    }
}
