using Sandbox;

public sealed class WeaponBase : BaseComponent
{
	// In future we should create the viewmodel from a prefab, this is just temporary
	[Property] SkinnedModelRenderer vModel { get; set; }

	public GameObject Eye;
	public Angles EyeAngles;

	public override void OnEnabled() {
		base.OnEnabled();
		Eye = GameObject.Parent.GetComponent<RWPlayerController>().Eye;
		Log.Info( Eye );
	}

	public override void Update()
	{
		if ( Input.Pressed( "attack1" ) )
		{
			var eyepos = Transform.Position + Vector3.Up * 60;
			Log.Info( eyepos + EyeAngles.ToRotation().Forward * 1000.0f );

			var tr = Physics.Trace.Ray( eyepos, eyepos + EyeAngles.ToRotation().Forward * 1000.0f )
				.WithoutTags( "physics" )
				.Run();

			if ( vModel is not null )
			{
				vModel.Set( "b_attack", true );
				Sound.FromWorld( "pistolfire", vModel.GameObject.Transform.Position );
			}

		}
	}
}
