using Amazon.CDK;
using Integration.PriorAuth.Core.Config;
using Integration.PriorAuth.Infra.Base.Helpers;

namespace Tests
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new TestsStack(app, $"{ConfigHelper.AppRoot}-TestsStack",new StackProps { Env = EnvironmentHelper.MakeEnvironment() });

            app.Synth();
        }
    }
}
