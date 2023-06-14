using Integration.PriorAuth.Infra.Base.Helpers.Enums;
using Environment = Integration.PriorAuth.Infra.Base.Helpers.Enums.Environment;

namespace Integration.PriorAuth.Infra.Base.Helpers;

public class BaseTaggingOptions
{
    #region Properties

    public string Name { get; set; } = "Integration-PriorAuth";

    public DataClassification DataClassification { get; set; } = DataClassification.PHI;

    public string Context { get; set; } = "Integration-PriorAuth";

    public Environment Environment { get; set; } = Environment.sandbox;

    public string UserName
    {
        get
        {
            if (!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("USERNAME")))
            {
                return System.Environment.GetEnvironmentVariable("USERNAME") ?? "vnp";
            }
            else
            {
                return "PriorAuth-Integration-Team";
            }
        }
    }

    public string OwningTeam { get; set; } = "NUCLEUS-POD16";

    public bool SafeShutdown { get; set; } = false;

    public string BusinessOwner { get; set; } = "PriorAuth-Integration-Team@navitus.com";

    public Compliance Compliance { get; set; } = Compliance.vnp;

    public string Notification { get; set; } = "PriorAuth-Integration-Team@navitus.com";

    public OS OS { get; set; } = OS.vnp;

    #endregion End Properties
}
