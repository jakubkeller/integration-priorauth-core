using Amazon.CDK;
using Amazon.CDK.AWS.Events;
using Constructs;
using Amazon.CDK.AWS.SSM;
using Integration.Banjo.Base;
using Integration.Banjo.Core.Config;

namespace Integration.Banjo.Core.Stack;

public class CoreStack : BaseStack
{
    internal CoreStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        const string application = "banjo-integration";
        const string banjoSource = $"com.navitus.{application}";
        var stackName = Amazon.CDK.Stack.Of(this).StackName;
        var environment = ConfigHelper.GetCurrentEnvironment();
        var ssmPathRoot = $"/app/{stackName}/";
        var banjoEventBus = new EventBus(this, "BanjoEventBus", new EventBusProps() { EventBusName = $"banjo-{environment.Name}-event-bus" });

        new CfnEventBusPolicy(this, "BanjoEventBusPolicy", new CfnEventBusPolicyProps()
        {
            Action = "events:PutEvents",
            EventBusName = banjoEventBus.EventBusName,
            Principal = environment.MatchingUMAccount,
            StatementId = $"EnableCrossAccountEventsFromUM{environment.MatchingUMAccount}"
        });

        new CrossAccountEventBridgeSync(
            this,
            "CrossAccountEventBridgeSync",
            new CrossAccountEventBridgeSyncProps()
            {
                ProducerEventBus = banjoEventBus,
                Source = banjoSource,
                ConsumerEventBusArns = new[]
                {
                    $"arn:{Aws.PARTITION}:events:{Aws.REGION}:{environment.MatchingUMAccount}:event-bus/umEventBus"
                }
            }
        );

        new EventLogForSource(this, "BanjoEventLog", new EventLogForSourceProps() {
            EventBus = banjoEventBus,
            Source = banjoSource
        });

        new EventLogForSource(this, "UMEventLog", new EventLogForSourceProps() {
            EventBus = banjoEventBus,
            Source = "com.navitus.um"
        });

        new StringParameter(this, "BanjoEventbusArnSSMParameter", new StringParameterProps
        {
            Description = "The arn of the Banjo Event Bus",
            ParameterName = $"{ssmPathRoot}BanjoEventBusArn",
            StringValue = banjoEventBus.EventBusArn,
            Tier = ParameterTier.STANDARD,
        });
    }
}
