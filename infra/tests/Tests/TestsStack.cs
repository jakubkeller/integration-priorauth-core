using Amazon.CDK;
using Amazon.CDK.AWS.SNS;
using Amazon.CDK.AWS.SNS.Subscriptions;
using Amazon.CDK.AWS.SQS;
using Constructs;

using Integration.PriorAuth.Infra.Base;

namespace Tests
{
    public class TestsStack : BaseStack
    {
        internal TestsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props, applicationName: "test", forceDisableNagPack: true)
        {
             // The CDK includes built-in constructs for most resource types, such as Queues and Topics.
            var queue = new Queue(this, "TestsQueue", new QueueProps
            {
                VisibilityTimeout = Duration.Seconds(300)
            });

            var topic = new Topic(this, "TestsTopic");

            topic.AddSubscription(new SqsSubscription(queue));
        }
    }
}
