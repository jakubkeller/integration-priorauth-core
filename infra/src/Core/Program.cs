using Amazon.CDK;
using Integration.PriorAuth.Infra.Base.Helpers;
using Integration.PriorAuth.Infra.Base.Config;
using Integration.PriorAuth.Core.Stack;

namespace Integration.PriorAuth.Core;

sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();

        new CoreStack(
            app,
            $"{ConfigHelper.AppRoot}-CoreStack",
            new StackProps { Env = EnvironmentHelper.MakeEnvironment() }
        );

        app.Synth();
    }
}
