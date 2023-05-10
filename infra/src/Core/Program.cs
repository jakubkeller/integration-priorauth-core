using Amazon.CDK;
using Core;
using Core.Helpers;
using Nucleus.Nagpack;
using System.Collections.Generic;

namespace Infra
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            var env = EnvironmentHelper.MakeEnvironment();

            new CoreStack(app, "CoreStack", new StackProps
            {
                Env = env
            });

            Aspects.Of(app).Add(new NucleusNagpack());

            app.Synth();
        }
    }
}
