using Amazon.CDK;
using Constructs;

namespace Integration.PriorAuth.Infra.Base.Helpers;

public static class TaggingHelper
{
    public static void SetBaseTags(Construct construct, string applicationName, BaseTaggingOptions taggingOptions)
    {
        AddTag(construct, "Application", applicationName, new TagProps { Priority = 10 });
        AddTag(construct, "Name", taggingOptions.Name, new TagProps { Priority = 10 });
        AddTag(construct, "DataClassification", taggingOptions.DataClassification.ToString(), new TagProps { Priority = 10 });
        AddTag(construct, "Context", taggingOptions.Context, new TagProps { Priority = 10 });
        AddTag(construct, "Environment", taggingOptions.Environment.ToString(), new TagProps { Priority = 10 });
        AddTag(construct, "Creator", taggingOptions.UserName, new TagProps { Priority = 10 });
        AddTag(construct, "OwningTeam", taggingOptions.OwningTeam, new TagProps { Priority = 10 });
        AddTag(construct, "SafeToShutdown", taggingOptions.SafeShutdown.ToString(), new TagProps { Priority = 10 });
        AddTag(construct, "BusinessOwner", taggingOptions.BusinessOwner, new TagProps { Priority = 10 });
        AddTag(construct, "Compliance", taggingOptions.Compliance.ToString(), new TagProps { Priority = 10 });
    }

    public static void AddTag(Construct construct, string key, string value)
    {
        Amazon.CDK.Tags.Of(construct).Add(key, value);
    }

    public static void AddTag(Construct construct, string key, string value, TagProps tagProp)
    {
        Amazon.CDK.Tags.Of(construct).Add(key, value, tagProp);
    }

    public static void RemoveTag(Construct construct, string key)
    {
        Amazon.CDK.Tags.Of(construct).Remove(key);
    }

    public static void RemoveTag(Construct construct, string key, TagProps tagProp)
    {
        Amazon.CDK.Tags.Of(construct).Remove(key, tagProp);
    }
}


