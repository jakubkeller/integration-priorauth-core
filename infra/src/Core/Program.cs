using Amazon.CDK;
using Integration.Banjo.Base.Helpers;
using Integration.Banjo.Core.Config;
using Integration.Banjo.Core.Stack;

namespace Integration.Banjo.Core;

sealed class Program
{
    private static readonly string _localStackPrefix = System.Environment.GetEnvironmentVariable("LOCAL_STACK_PREFIX") ?? "IntegrationBanjo";
    public static void Main(string[] args)
    {
        var app = new App();
        
        // Test Feature Build

        var currentEnvironment = ConfigHelper.GetCurrentEnvironment(System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"));

        new CoreStack(app, $"{_localStackPrefix}-{currentEnvironment.Name}-CoreStack", new StackProps { Env = EnvironmentHelper.MakeEnvironment() });

        app.Synth();
    }
}
