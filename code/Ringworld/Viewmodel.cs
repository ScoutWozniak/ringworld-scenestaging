using Sandbox;

public sealed class Viewmodel : BaseComponent
{
	[Property] GameObject eye { get; set; }
	public override void Update()
	{
		var cam = Scene.GetComponent<CameraComponent>( true, true );
		GameObject.Transform.Position = cam.Transform.Position;
		GameObject.Transform.Rotation = cam.Transform.Rotation;
	}
}
