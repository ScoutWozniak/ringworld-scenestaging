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
		owner = GameObject.Parent.Components.Get<RWPlayerController>( FindMode.EverythingInSelfAndAncestors);
		Log.Info( owner );
	}

	protected override void OnAwake()
	{
		base.OnStart();
		owner = GameObject.Parent.Components.Get<RWPlayerController>( FindMode.EverythingInSelfAndDescendants );
		Log.Info( owner );
	}

	public SceneTraceResult TraceBullet()
	{
		var eyepos = owner.Transform.Position + (Vector3.Up * 75.0f);
		EyeAngles = owner.EyeAngles.ToRotation();

		var tr = Scene.Trace.Ray( eyepos, eyepos + EyeAngles.Forward * 1000.0f )
				.WithoutTags( "physics", "localplayer" )
				.UseHitboxes( true )
				.Run();


			return tr;
	}

	protected override void OnUpdate()
	{
		// This all needs to run only on the client, not a safe way but it's good enough
		if ( IsProxy )
			return;

		if ( Input.Pressed( "attack1" ) )
		{
			var tr = TraceBullet();
			
			Log.Info( tr.Body.GameObject );
				
			if (tr.Hitbox is not null)
			{
				Log.Info( tr.Hitbox.Bone );
			}
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

				var test2 = go.Components.Get<TargetTestComponent>();
				if (test2 is not null)
				{
					test2.OnHit();
				}
			}

			if ( vModel is not null )
			{
				vModel.Set( "b_attack", true );
			}

			//FireEffects();

		}
	}

	[Broadcast]
	void FireEffects() 
	{
		Sound.FromWorld( "pistolfire", vModel.GameObject.Transform.Position );
		Components.Get<SoundPointComponent>().StartSound();
	}
}
