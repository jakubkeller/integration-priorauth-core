using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Events;
using Core.Helpers;
using Constructs;
using System;
using Amazon.CDK.AWS.SSM;

namespace Core
{
    public class CoreStack : Stack
    {
        internal CoreStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var policy = ManagedPolicy.FromManagedPolicyName(this, "BoundaryPolicy", "DeveloperBoundaryPolicy");
            Amazon.CDK.AWS.IAM.PermissionsBoundary.Of(this).Apply(policy);

            TaggingHelper.SetBaseTags(this, "IntegrationBanjoCore", new BaseTaggingOptions {});

            var account = Stack.Of(this).Account;
            var region = Stack.Of(this).Region;

            Console.WriteLine($"Account ID: {account}\tRegion: {region}");

            const string EVENT_BUS_NAME = "Banjo";

            var banjoEventBus = new EventBus(this, "BanjoEventBus", new Amazon.CDK.AWS.Events.EventBusProps {
                EventBusName = EVENT_BUS_NAME,
            });

            new StringParameter(this, "BanjoEventbusArnSSMParameter", new StringParameterProps
            {
                Description = "The arn of the Banjo Event Bus",
                ParameterName = "BanjoEventBusArn",
                StringValue = banjoEventBus.EventBusArn,
                Tier = ParameterTier.STANDARD,
            });
        }
    }
}
