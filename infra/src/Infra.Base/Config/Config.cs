using System.Reflection;
using System.Text.Json;

namespace Integration.PriorAuth.Infra.Base.Config;

public class CommonConfig
{
    public List<Environment> Environments { get; set; } = new List<Environment>();

    public Environment GetSandboxEnvironment()
    {
        return Environments.First(e => e.Type == EnvironmentType.Sandbox);
    }
}

public class Environment
{
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string AccountId { get; set; } = default!;
    public string VpcName { get; set; } = default!;
    public string MatchingUMAccount { get; set; } = default!;
    public string[] NotificationEmailAddresses { get; set; } = Array.Empty<string>();

    public EnvironmentType Type
    {
        get
        {
            // map string to enum
            if (Name == "sandbox")
            {
                return EnvironmentType.Sandbox;
            }
            else if (Name == "dev")
            {
                return EnvironmentType.Dev;
            }
            else if (Name == "test")
            {
                return EnvironmentType.Test;
            }
            else if (Name == "stage")
            {
                return EnvironmentType.Stage;
            }
            else if (Name == "prod")
            {
                return EnvironmentType.Prod;
            }
            else
            {
                // throw exception if the environment is not supported
                throw new Exception($"Environment {Name} is not supported");
            }
        }
    }
}

public enum EnvironmentType
{
    Sandbox = 0,
    Dev = 1,
    Test = 2,
    Stage = 3,
    Prod = 4,
}

public static class ConfigHelper
{
    private static Environment? _currentEnvironment = null;
    public static string AppRoot =>
        System.Environment.GetEnvironmentVariable("LOCAL_APP_ROOT") ?? "integration-priorauth";

    public static Environment GetCurrentEnvironment(string? accountId = null)
    {
        if (_currentEnvironment != null)
        {
            return _currentEnvironment;
        }
        // read config json file from assembly location where nuget package is installed
        string assemblyDirectoryPath = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location
        )!;
        string configFilePath = Path.Combine(assemblyDirectoryPath, "config.json");

        var config = JsonSerializer.Deserialize<CommonConfig>(File.ReadAllText(configFilePath))!;
        if (string.IsNullOrEmpty(accountId))
        {
            // if environment variable is not set, use Sandbox as default
            accountId = config.GetSandboxEnvironment().AccountId;
        }
        Console.WriteLine($"AWS Account id: {accountId}");

        // read config json file into dictionary
        _currentEnvironment = config.Environments.FirstOrDefault(
            e => e.AccountId == accountId,
            config.GetSandboxEnvironment()
        );

        Console.WriteLine($"Current Environment: {_currentEnvironment.DisplayName}");
        return _currentEnvironment;
    }
}
