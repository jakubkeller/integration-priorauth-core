using Amazon.CDK;
using Amazon.CDK.AWS.Events;
using Constructs;
using Amazon.CDK.AWS.SSM;
using Integration.PriorAuth.Infra.Base;
using Integration.PriorAuth.Infra.Base.Config;
using Amazon.CDK.AWS.SecretsManager;
using System.Collections.Generic;

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

        _ = new CfnEventBusPolicy(
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

        _ = new CrossAccountEventBridgeSync(
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

        _ = new EventLogForSource(
            this,
            "IntegrationPriorAuthEventLog",
            new EventLogForSourceProps()
            {
                EventBus = integrationPriorAuthEventBus,
                Source = priorAuthSource
            }
        );

        _ = new EventLogForSource(
            this,
            "UMEventLog",
            new EventLogForSourceProps()
            {
                EventBus = integrationPriorAuthEventBus,
                Source = "com.navitus.um"
            }
        );

        _ = new StringParameter(
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

        _ = new StringParameter(
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

        // this is a secret used to store creds for Banjo AppClient
        // you will deploy this placeholder and manually update the secret in lower envs for testing
        // in higher env we will need someone else to provision this for us based on what banjo gives us
        _ = new Secret(
            this,
            "BanjoAppClientSecret",
            new SecretProps
            {
                SecretName = "AppClient/Banjo",
                Description = "Integration AppClient to Interact with Banjo APIs",
                SecretObjectValue = new Dictionary<string, SecretValue>
                {
                    { "clientId", SecretValue.UnsafePlainText("<< NOT A REAL CLIENT ID >>") },
                    { "clientSecret", SecretValue.UnsafePlainText("<< NOT A REAL SECRET >>") },
                    { "authUrl", SecretValue.UnsafePlainText("<< NOT A REAL AUTH URL >>") },
                    { "authScope", SecretValue.UnsafePlainText("<< NOT THE REAL AUTH SCOPE >>") },
                    { "code", SecretValue.UnsafePlainText("<< NOT THE REAL CODE >>") }
                }
            }
        );

        Amazon.CDK.Tags
            .Of(this)
            .Add("Name", "Integration-PriorAuth-Core", new TagProps { Priority = 20 });
    }
}
