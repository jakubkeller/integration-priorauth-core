using Amazon.CDK;
using Constructs;

namespace Core.Helpers
{
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
            AddTag(construct, "Department", taggingOptions.Department, new TagProps { Priority = 10 });
            AddTag(construct, "OwningTeam", taggingOptions.OwningTeam, new TagProps { Priority = 10 });
            AddTag(construct, "SafeToShutdown", taggingOptions.SafeShutdown.ToString(), new TagProps { Priority = 10 });
            AddTag(construct, "AWSBackup ", taggingOptions.AwsBackup, new TagProps { Priority = 10 });
            AddTag(construct, "BusinessOwner", taggingOptions.BusinessOwner, new TagProps { Priority = 10 });
            AddTag(construct, "CostCenter", taggingOptions.CostCenter, new TagProps { Priority = 10 });
            AddTag(construct, "map-migrated", taggingOptions.MapMigrated, new TagProps { Priority = 10 });
            AddTag(construct, "map-dba", taggingOptions.MapDba, new TagProps { Priority = 10 });
            AddTag(construct, "aws-migration-project-id", taggingOptions.AwsMigrationProjectId, new TagProps { Priority = 10 });
            AddTag(construct, "Compliance", taggingOptions.Compliance.ToString(), new TagProps { Priority = 10 });
            AddTag(construct, "DataAccess", taggingOptions.DataAccess, new TagProps { Priority = 10 });
            AddTag(construct, "Consumer", taggingOptions.Customer.ToString(), new TagProps { Priority = 10 });
            AddTag(construct, "CustomerType", taggingOptions.CustomerType.ToString(), new TagProps { Priority = 10 });
            AddTag(construct, "Version", taggingOptions.Version, new TagProps { Priority = 10 });
            AddTag(construct, "ServiceRequest", taggingOptions.ServiceRequest, new TagProps { Priority = 10 });
            AddTag(construct, "MaintainanceWindow", taggingOptions.MaintainanceWindow, new TagProps { Priority = 10 });
            AddTag(construct, "Notification", taggingOptions.Notification, new TagProps { Priority = 10 });
            AddTag(construct, "OS", taggingOptions.OS.ToString(), new TagProps { Priority = 10 });
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

}
