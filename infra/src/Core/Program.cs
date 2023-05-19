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
        
        new CoreStack(app, $"{ConfigHelper.AppRoot}-{ConfigHelper.GetCurrentEnvironment().Name}-CoreStack", new StackProps { Env = EnvironmentHelper.MakeEnvironment() });

        app.Synth();
    }
}
