using Sandbox;

public sealed class TargetTestComponent : BaseComponent
{
	[Property] AnimationTransformComponent testAnimPlayer { get; set; }

	public void OnHit()
	{
		testAnimPlayer.PlayAnim();
	}
}
