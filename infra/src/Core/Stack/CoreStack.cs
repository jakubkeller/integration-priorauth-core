using Amazon.CDK;
using Amazon.CDK.AWS.Events;
using Constructs;
using Amazon.CDK.AWS.SSM;
using Integration.PriorAuth.Infra.Base;
using Integration.PriorAuth.Infra.Base.Config;

namespace Integration.PriorAuth.Core.Stack;

public class CoreStack : BaseStack
{
    private const string Application = "integration.priorauth";

    internal CoreStack(Construct scope, string id, IStackProps props = null)
        : base(scope, id, props, Application)
    {
        const string priorAuthSource = $"com.navitus.{Application}";
        var stackName = Of(this).StackName;
        var ssmPathRoot = $"/{AppRoot}/core";
        var isSandbox = EnvironmentType.Sandbox.Equals(CurrentEnvironment.Type);

        var alarmTopic = new AlarmTopic(
            this,
            "IntegrationPriorAuthAlarmTopic",
            new AlarmTopicProps()
            {
                IsSandbox = isSandbox,
                BaseEnvironmentVariables = CurrentEnvironment,
                DisplayName = $"int-priorauth-core-{CurrentEnvironment.Name}-alarm-topic",
            }
        );

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
                    $"{base.AppRoot}-EnableEventsFromUMContext{CurrentEnvironment.MatchingUMAccount}"
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
                ParameterName = $"{ssmPathRoot}/IntegrationPriorAuthEventBusArn",
                StringValue = integrationPriorAuthEventBus.EventBusArn,
                Tier = ParameterTier.STANDARD,
            }
        );

        new StringParameter(
            this,
            "IntegrationPriorAuthAlarmTopicArnSSMParameter",
            new StringParameterProps
            {
                Description =
                    "The arn of the Integration PriorAuth Core Alarm Topic which generates alerts for the Utilization Management team to respond to.",
                ParameterName = $"{ssmPathRoot}/IntegrationPriorAuthAlarmTopicArn",
                StringValue = alarmTopic.Topic.TopicArn,
                Tier = ParameterTier.STANDARD,
            }
        );

        Amazon.CDK.Tags
            .Of(this)
            .Add("Name", "Integration-PriorAuth-Core", new TagProps { Priority = 20 });
    }
}
