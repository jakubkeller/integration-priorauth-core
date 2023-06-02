using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.IAM;
using Targets = Amazon.CDK.AWS.Events.Targets;
using Constructs;

public class CrossAccountEventBridgeSyncProps
{
    public string Source { get; set; }
    public string[] ConsumerEventBusArns { get; set; }
    public EventBus ProducerEventBus { get; set; }
}

public class CrossAccountEventBridgeSync : Construct
{
    public CrossAccountEventBridgeSync(
        Construct scope,
        string id,
        CrossAccountEventBridgeSyncProps props
    )
        : base(scope, id)
    {
        var crossAccountEventBridgePublishRole = new Role(
            this,
            "CrossAccountEventBridgePublish",
            new RoleProps() { AssumedBy = new ServicePrincipal("events.amazonaws.com") }
        );

        crossAccountEventBridgePublishRole.AddManagedPolicy(
            new ManagedPolicy(
                this,
                "AllowCrossAccountEventBridgePublish",
                new ManagedPolicyProps()
                {
                    Statements = new PolicyStatement[]
                    {
                        new PolicyStatement(
                            new PolicyStatementProps()
                            {
                                Sid =
                                    $"AllowCrossAccountEventBridgePutEvents{props.ConsumerEventBusArns[0].Split(":")[4]}",
                                Effect = Effect.ALLOW,
                                Actions = new[] { "events:PutEvents" },
                                Resources = props.ConsumerEventBusArns,
                            }
                        )
                    }
                }
            )
        );

        var crossAccountPublishRule = new Rule(
            this,
            "CrossAccountPublishRule",
            new RuleProps()
            {
                EventBus = props.ProducerEventBus,
                EventPattern = new EventPattern() { Source = new[] { props.Source } }
            }
        );

        for (int i = 0; i < props.ConsumerEventBusArns.Length; i++)
        {
            var eventBus = EventBus.FromEventBusArn(
                this,
                $"ConsumerEventBus-{i}",
                props.ConsumerEventBusArns[i]
            );

            crossAccountPublishRule.AddTarget(
                new Targets.EventBus(
                    eventBus,
                    new Targets.EventBusProps() { Role = crossAccountEventBridgePublishRole }
                )
            );
        }
    }
}
