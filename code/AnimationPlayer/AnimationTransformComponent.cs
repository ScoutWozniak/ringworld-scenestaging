using Sandbox;

public sealed class AnimationTransformComponent : BaseComponent
{
	[Property] GameObject animated { get; set; }

	[Property] Transform finalTransform { get; set; }

	[Property] bool useLocalTransform { get; set; }

	[Property] float animLength { get;set; }

	[Property] bool isLooped { get; set; }
	[Property] bool isPingPong { get; set; }

	[Property] bool playOnStart { get; set; }

	bool playing = false;

	Transform initialPos;

	TimeSince animCurrentTime;
	int pingPongs = 0;

	protected override void OnStart()
	{
		base.OnStart();
		if ( playOnStart ) PlayAnim();
	}

	protected override void OnUpdate()
	{
		if ( !playing ) return;

		if ( animCurrentTime.Relative < animLength )
		{
			float animPos = animCurrentTime.Relative / (animLength);
			animated.Transform.Position =	initialPos.Position.LerpTo( finalTransform.Position, animPos );
			animated.Transform.Rotation =	Rotation.Lerp(initialPos.Rotation, finalTransform.Rotation, animPos );
			animated.Transform.Scale =		initialPos.Scale.LerpTo( finalTransform.Scale, animPos );
		}
		else
		{
			if (isPingPong && isLooped || isPingPong && pingPongs < 1)
				PingPong();
			else if (isLooped)
				RestartAnim();
		}
	}

	void PingPong()
	{
		Transform tempPos = initialPos;
		initialPos = finalTransform;
		finalTransform = tempPos;
		animCurrentTime = 0;
		pingPongs += 1;
	}

	public void PlayAnim()
	{
		if ( playing )
			return;
		initialPos = animated.Transform.World;
		animCurrentTime = 0;
		if ( useLocalTransform )
		{
			finalTransform = finalTransform.WithPosition( initialPos.Position + finalTransform.Position );
		}
		playing = true;
	}

	public void RestartAnim()
	{
		animated.Transform.Position = initialPos.Position;
		animated.Transform.Rotation = initialPos.Rotation;
		animated.Transform.Scale = initialPos.Scale;
		animCurrentTime = 0;
		playing = true;
	}
}
