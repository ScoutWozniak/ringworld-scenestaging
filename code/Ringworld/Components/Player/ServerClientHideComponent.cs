using Sandbox;

public sealed class ServerClientHideComponent : BaseComponent
{
	[Property] public GameObject hideOnClient { get; set; }
	[Property] public GameObject hideOnServer { get; set; }


	protected override void OnEnabled()
	{
		base.OnEnabled();
		foreach ( GameObject go in hideOnClient.GetAllObjects( false ) )
		{
			go.Enabled = IsProxy;
		}

		foreach ( GameObject go in hideOnServer.GetAllObjects( false ) )
		{
			go.Enabled = !IsProxy;
		}
	}
}
