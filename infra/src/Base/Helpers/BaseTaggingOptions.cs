using Integration.Banjo.Base.Helpers.Enums;
using Environment = Integration.Banjo.Base.Helpers.Enums.Environment;

namespace Integration.Banjo.Base.Helpers;


public class BaseTaggingOptions
{
    #region Properties

    public string Name { get; set; } = "IntegrationBanjoCore";

    public DataClassification DataClassification { get; set; } = DataClassification.Clear;

    public string Context { get; set; } = "Core";

    public Environment Environment { get; set; } = Environment.sandbox;

    public string UserName
    {
        get
        {
            if (!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("USERNAME")))
            {
                return System.Environment.GetEnvironmentVariable("USERNAME");
            }
            else
            {
                return "Banjo-Integration-Team";
            }
        }
    }

    public string Department { get; set; } = "vnp";

    public string OwningTeam { get; set; } = "NUCLEUS-POD12";

    public bool SafeShutdown { get; set; } = false;

    public string AwsBackup { get; set; } = "vnp";

    public string BusinessOwner { get; set; } = "Banjo-Integration-Team@navitus.com";

    public string CostCenter { get; set; } = "vnp";

    public string MapMigrated { get; set; } = "vnp";

    public string MapDba { get; set; } = "vnp";

    public string AwsMigrationProjectId { get; set; } = "vnp";

    public Compliance Compliance { get; set; } = Compliance.vnp;

    public string DataAccess { get; set; } = "vnp";

    public Customer Customer { get; set; } = Customer.vnp;

    public CustomerType CustomerType { get; set; } = CustomerType.vnp;

    public string Version { get; set; } = "vnp";

    public string ServiceRequest { get; set; } = "vnp";

    public string MaintainanceWindow { get; set; } = "vnp";

    public string Notification { get; set; } = "Banjo-Integration-Team@navitus.com";

    public OS OS { get; set; } = OS.vnp;

    #endregion End Properties
}
