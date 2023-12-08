using Sandbox;

public sealed class TargetTestComponent : Component
{
	[Property] AnimationTransformComponent testAnimPlayer { get; set; }

	public void OnHit()
	{
		testAnimPlayer.PlayAnim();
	}
}
