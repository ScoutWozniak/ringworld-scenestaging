using Sandbox;

public sealed class Viewmodel : Component
{
	[Property] GameObject eye { get; set; }
	protected override void OnUpdate()
	{
		var cam = Scene.Components.Get<CameraComponent>( Sandbox.FindMode.EverythingInSelfAndDescendants );
		GameObject.Transform.Position = cam.Transform.Position;
		GameObject.Transform.Rotation = cam.Transform.Rotation;
		
	}
}
