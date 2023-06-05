using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Logs;
using Targets = Amazon.CDK.AWS.Events.Targets;
using Constructs;

public class EventLogForSourceProps
{
    public EventBus EventBus;
    public string Source;
}

public class EventLogForSource : Construct
{
    public EventLogForSource(Construct scope, string id, EventLogForSourceProps props)
        : base(scope, id)
    {
        var logGroup = new LogGroup(
            this,
            "LogGroup",
            new LogGroupProps() { Retention = RetentionDays.ONE_MONTH }
        );

        var rule = new Rule(
            this,
            "Rule",
            new RuleProps()
            {
                EventBus = props.EventBus,
                EventPattern = new EventPattern() { Source = new[] { props.Source } }
            }
        );

        rule.AddTarget(new Targets.CloudWatchLogGroup(logGroup));
    }
}
