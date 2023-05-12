using Amazon.CDK;
using Amazon.CDK.AWS.Events;
using Constructs;
using Amazon.CDK.AWS.SSM;
using Integration.Banjo.Base;

namespace Integration.Banjo.Core.Stack;

public class CoreStack : BaseStack
{
    internal CoreStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {

        const string EVENT_BUS_NAME = "Banjo";
        var stackName = Node.TryGetContext("stackName")?.ToString() ?? null;
        var ssmPathRoot = $"/app/{stackName}";

        var banjoEventBus = new EventBus(this, "BanjoEventBus", new EventBusProps {
            EventBusName = EVENT_BUS_NAME,
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
