using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Constructs;
using Integration.Banjo.Base.Helpers;
using Integration.Banjo.Core.Config;
using Nucleus.Nagpack;
using Environment = Integration.Banjo.Core.Config.Environment;

namespace Integration.Banjo.Base
{
	public class BaseStack : Stack
	{
		protected Environment CurrentEnvironment { get; }

		public BaseStack(Construct scope, string id, IStackProps props = null, string applicationName="vnp", bool forceDisableNagPack = false) : base(scope, id, props)
		{
			var policy = ManagedPolicy.FromManagedPolicyName(this, "BoundaryPolicy", "DeveloperBoundaryPolicy");
			Amazon.CDK.AWS.IAM.PermissionsBoundary.Of(this).Apply(policy);

			var account = Stack.Of(this).Account;
			CurrentEnvironment = ConfigHelper.GetCurrentEnvironment(account);

			// NagPack should not be disabled. But if you need to try out some changes quickly without dealing with nags, use this flag.
			if(!forceDisableNagPack)
			{
				Aspects.Of(this).Add(new NucleusNagpack());
			}

			// set tags
			TaggingHelper.SetBaseTags(this, applicationName, new BaseTaggingOptions {});
		}
	}
}
