using Amazon.CDK;

namespace Integration.Banjo.Base.Helpers;

public static class EnvironmentHelper
{
    public static IEnvironment MakeEnvironment(string account = null, string region = null)
    {
        return new Amazon.CDK.Environment
        {
            Account = account?.Length > 0 ? account : account ??
                System.Environment.GetEnvironmentVariable("CDK_DEPLOY_ACCOUNT") ??
                System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT") ?? "822596811112",
            Region = region?.Length > 0 ? region : region ??
                System.Environment.GetEnvironmentVariable("CDK_DEPLOY_REGION") ??
                System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION") ?? "us-east-2"
        };
    }
}