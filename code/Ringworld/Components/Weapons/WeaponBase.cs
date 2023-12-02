using Sandbox;
using System.Diagnostics;

public sealed class WeaponBase : BaseComponent
{
	// In future we should create the viewmodel from a prefab, this is just temporary
	[Property] SkinnedModelRenderer vModel { get; set; }

	public GameObject Eye;
	public Rotation EyeAngles;

	RWPlayerController owner;

	[Property] int damage { get; set; }

	protected override void OnEnabled() {
		base.OnEnabled();
		owner = GameObject.Parent.Components.Get<RWPlayerController>( FindMode.EverythingInSelfAndDescendants );

	}

	public IEnumerable<PhysicsTraceResult> TraceBullet()
	{
		var eyepos = owner.Transform.Position + (Vector3.Up * 75.0f);
		EyeAngles = owner.EyeAngles.ToRotation();

		var tr = Physics.Trace.Ray( eyepos, eyepos + EyeAngles.Forward * 1000.0f )
				.WithoutTags( "physics", "localplayer" )
				.Run();

		if ( tr.Hit )
		{
			yield return tr;
		}

	}

	protected override void OnUpdate()
	{
		// This all needs to run only on the client, not a safe way but it's good enough
		if ( IsProxy )
			return;

		if ( Input.Pressed( "attack1" ) )
		{
			Log.Info( TraceBullet().Count() );
			foreach ( var tr in TraceBullet() )
			{
				
				Log.Info( tr.Body.GameObject );
				if ( tr.Body.GameObject is GameObject go ) 
				{
					var test = go.Components.Get<Health>( FindMode.InAncestors );
					if ( test is not null && test.GameObject.IsProxy )
					{
						Log.Info( $"Found a player at {test.GameObject}" );
						test.curHp -= 1;
						Log.Info( test.curHp );
						test.Hurt( damage );
					}
				}
			}

			if ( vModel is not null )
			{
				vModel.Set( "b_attack", true );
			}

			FireEffects();

		}
	}

	[Broadcast]
	void FireEffects() 
	{
		Sound.FromWorld( "pistolfire", vModel.GameObject.Transform.Position );
		Components.Get<SoundPointComponent>().StartSound();
	}
}
