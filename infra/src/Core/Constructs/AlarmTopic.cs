using Amazon.CDK.AWS.KMS;
using Amazon.CDK.AWS.SNS;
using Amazon.CDK.AWS.SNS.Subscriptions;
using Constructs;

public class AlarmTopicProps
{
    public bool IsSandbox { get; set; } = default!;
    public Integration.PriorAuth.Infra.Base.Config.Environment BaseEnvironmentVariables { get; set; } = default!;

    public string DisplayName { get; set; } = default!;
}

public class AlarmTopic : Construct
{
    public Topic Topic { get; private set; }

    public AlarmTopic(Construct scope, string id, AlarmTopicProps props)
        : base(scope, id)
    {
        var snsKmsKey = Key.FromLookup(
            this,
            "SNSKey",
            new KeyLookupOptions { AliasName = "alias/SNSKey" }
        );

        Topic = new Topic(
            this,
            id,
            new TopicProps() { MasterKey = snsKmsKey, DisplayName = props.DisplayName }
        );

        if (!props.IsSandbox)
        {
            foreach (var address in props.BaseEnvironmentVariables.NotificationEmailAddresses)
            {
                Topic.AddSubscription(new EmailSubscription(address));
            }
        }
    }
}
