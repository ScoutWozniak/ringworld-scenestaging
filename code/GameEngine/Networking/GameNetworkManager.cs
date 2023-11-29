using Sandbox;
using System.Security.Cryptography;

public sealed class GameNetworkManager : BaseComponent, BaseComponent.INetworkListener
{
	[Property] public GameObject PlayerPrefab { get; set; }
	[Property] public GameObject SpawnPoint { get; set; }

	[Property] public GameObject HudExample { get; set; }

	public override void OnStart()
	{

	}

	public override void Update()
	{
		
	}

	public void OnActive( Connection channel )
	{
		Log.Info( $"Player '{channel.DisplayName}' is becoming active" );

		var player = SceneUtility.Instantiate( PlayerPrefab, SpawnPoint.Transform.World );

		var nameTag = player.GetComponent<NameTagPanel>( false, true );
		if ( nameTag is not null )
		{
			nameTag.Name = channel.DisplayName;
		}


		player.Network.Spawn( channel );
	}
}
