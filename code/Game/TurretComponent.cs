using Sandbox;
using Sandbox.Diagnostics;
using Sandbox.Services;
using System;

public sealed class TurretComponent : BaseComponent
{
	[Property] GameObject Gun { get; set; }
	[Property] GameObject Bullet { get; set; }
	[Property] GameObject SecondaryBullet { get; set; }
	[Property] GameObject Muzzle { get; set; }

	float turretYaw;
	float turretPitch;

	TimeSince timeSinceLastSecondary;

	public override void Update()
	{
		// rotate gun using mouse input
		turretYaw -= Input.MouseDelta.x * 0.1f;
		turretPitch += Input.MouseDelta.y * 0.1f;
		turretPitch = turretPitch.Clamp( -30, 30 );
		Gun.Transform = Gun.Transform.WithRotation( Rotation.From( turretPitch, turretYaw, 0  ) );

		// drive tank
		Vector3 movement = 0;
		if ( Input.Down( "Forward" ) ) movement += GameObject.WorldTransform.Forward;
		if ( Input.Down( "backward" ) ) movement += GameObject.WorldTransform.Backward;

		var rot = GameObject.Transform.Rotation;
		var pos = GameObject.Transform.Position + movement * Time.Delta * 100.0f;

		if ( Input.Down( "Left" ) )
		{
			rot *= Rotation.From( 0, Time.Delta * 90.0f, 0 );
		}

		if ( Input.Down( "Right" ) )
		{
			rot *= Rotation.From( 0, Time.Delta * -90.0f, 0 );
		}

		GameObject.Transform = new Transform( pos, rot, 1 );

		if ( Input.Pressed( "Attack1" ) )
		{
			Assert.NotNull( Bullet );

			var obj = SceneUtility.Instantiate( Bullet, Muzzle.WorldTransform.Position, Muzzle.WorldTransform.Rotation );
			var physics = obj.GetComponent<PhysicsComponent>( true, true );
			if ( physics is not null )
			{
				physics.Velocity = Muzzle.WorldTransform.Rotation.Forward * 2000.0f;
			}

			Stats.Increment( "balls_fired", 1 );
		}

		var tr = Physics.Trace
			.Ray( Muzzle.WorldTransform.Position, Muzzle.WorldTransform.Position + Muzzle.WorldTransform.Forward * 4000 )
			.Run();

		Gizmo.Transform = Transform.Zero;
		Gizmo.Draw.Color = Color.White;
		Gizmo.Draw.LineThickness = 1;
		Gizmo.Draw.Line( tr.StartPosition, tr.EndPosition );
		Gizmo.Draw.Line( tr.HitPosition, tr.HitPosition + tr.Normal * 30.0f );

		using ( Gizmo.Scope( "circle", new Transform( tr.HitPosition, Rotation.LookAt( tr.Normal ) ) ) )
		{
			Gizmo.Draw.LineCircle( 0, 30 );
		}

		if ( Input.Pressed( "Attack2" ) && timeSinceLastSecondary > 0.02f && tr.Hit )
		{
			Stats.Increment( "cubes_fired", 1 );

			timeSinceLastSecondary = 0;

			int i = 0;

			var r = Muzzle.WorldTransform.Rotation;

			for ( float f = 0; f < tr.Distance; f += 10.0f )
			{
				if ( i++ > 200 )
					break;

				var off = MathF.Sin( i * 0.4f ) * Muzzle.WorldTransform.Right * 20.0f;
				off += MathF.Cos( i * 0.4f ) * Muzzle.WorldTransform.Up * 20.0f;

				var obj = SceneUtility.Instantiate( SecondaryBullet, tr.StartPosition + tr.Direction * f + off * 0.1f, r );

				//r *= Rotation.From( 2, 4, 2 );

				var physics = obj.GetComponent<PhysicsComponent>( true, true );
				if ( physics is not null )
				{
					physics.Velocity = off * 2.0f;// Muzzle.WorldTransform.Rotation.Forward * 300.0f;
				}
			}
	
		}
	}
}
