using Amazon.CDK;
using Amazon.CDK.AWS.Events;
using Constructs;
using Amazon.CDK.AWS.SSM;
using Integration.PriorAuth.Infra.Base;

namespace Integration.PriorAuth.Core.Stack;

public class CoreStack : BaseStack
{
    internal CoreStack(Construct scope, string id, IStackProps props = null)
        : base(scope, id, props)
    {
        const string application = "integration.priorauth";
        const string priorAuthSource = $"com.navitus.{application}";
        var stackName = Amazon.CDK.Stack.Of(this).StackName;
        var ssmPathRoot = $"/app/{stackName}/";
        var integrationPriorAuthEventBus = new EventBus(
            this,
            "IntegrationPriorAuthEventBus",
            new EventBusProps() { EventBusName = $"{stackName}-event-bus" }
        );

        new CfnEventBusPolicy(
            this,
            "IntegrationPriorAuthEventBusPolicy",
            new CfnEventBusPolicyProps()
            {
                Action = "events:PutEvents",
                EventBusName = integrationPriorAuthEventBus.EventBusName,
                Principal = CurrentEnvironment.MatchingUMAccount,
                StatementId =
                    $"EnableCrossAccountEventsFromUMContext{CurrentEnvironment.MatchingUMAccount}"
            }
        );

        new CrossAccountEventBridgeSync(
            this,
            "CrossAccountEventBridgeSync",
            new CrossAccountEventBridgeSyncProps()
            {
                ProducerEventBus = integrationPriorAuthEventBus,
                Source = priorAuthSource,
                ConsumerEventBusArns = new[]
                {
                    $"arn:{Aws.PARTITION}:events:{Aws.REGION}:{CurrentEnvironment.MatchingUMAccount}:event-bus/umEventBus"
                }
            }
        );

        new EventLogForSource(
            this,
            "IntegrationPriorAuthEventLog",
            new EventLogForSourceProps()
            {
                EventBus = integrationPriorAuthEventBus,
                Source = priorAuthSource
            }
        );

        new EventLogForSource(
            this,
            "UMEventLog",
            new EventLogForSourceProps()
            {
                EventBus = integrationPriorAuthEventBus,
                Source = "com.navitus.um"
            }
        );

        new StringParameter(
            this,
            "IntegrationPriorAuthEventbusArnSSMParameter",
            new StringParameterProps
            {
                Description = "The arn of the Integration PriorAuth Event Bus",
                ParameterName = $"{ssmPathRoot}IntegrationPriorAuthEventBusArn",
                StringValue = integrationPriorAuthEventBus.EventBusArn,
                Tier = ParameterTier.STANDARD,
            }
        );
    }
}
