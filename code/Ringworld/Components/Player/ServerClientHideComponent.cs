using Sandbox;

public sealed class ServerClientHideComponent : BaseComponent
{
	[Property] public bool disable { get; set; }
	[Property] public GameObject hideOnClient { get; set; }
	[Property] public GameObject hideOnServer { get; set; }

	[Property] public GameObject tempCollider { get; set; }
	protected override void OnEnabled()
	{
		base.OnEnabled();
		
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		if ( disable )
			return;

		foreach ( GameObject go in hideOnClient.GetAllObjects( false ) )
		{
			go.Enabled = IsProxy;
		}

		foreach ( GameObject go in hideOnServer.GetAllObjects( false ) )
		{
			go.Enabled = !IsProxy;

		}

		if (!IsProxy)
		{
			tempCollider.Tags.Add( "localplayer" );
		}
		
	}
}
