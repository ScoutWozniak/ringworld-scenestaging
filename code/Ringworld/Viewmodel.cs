using Sandbox;

public sealed class Viewmodel : BaseComponent
{
	[Property] GameObject eye { get; set; }
	protected override void OnUpdate()
	{
		var cam = Scene.Components.Get<CameraComponent>( FindMode.EverythingInSelfAndDescendants );
		GameObject.Transform.Position = cam.Transform.Position;
		GameObject.Transform.Rotation = cam.Transform.Rotation;
		
	}
}
