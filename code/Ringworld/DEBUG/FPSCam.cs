using Sandbox;

public sealed class FPSCam : Component
{
	protected override void OnUpdate()
	{
		if ( IsProxy )
			return;

		var cam = Scene.Components.Get<CameraComponent>( FindMode.EverythingInSelfAndDescendants );
		cam.GameObject.Transform.Position = GameObject.Transform.Position;
	}
}
