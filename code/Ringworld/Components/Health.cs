using Sandbox;

public sealed class Health : BaseComponent, INetworkSerializable
{
	[Property] public int maxShieldPoints { get; set; } = 70;
	[Property] public int maxHealthPoints { get; set; } = 45;

	[Property] public int curSp { get; set; }
	[Property] public int curHp { get; set; }

	protected override void OnAwake()
	{
		base.OnAwake();
		curSp = maxShieldPoints;
		curHp = maxHealthPoints;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	public void Write( ref ByteStream stream )
	{
		stream.Write( curSp );
		stream.Write( curHp );
	}

	public void Read( ByteStream stream )
	{
		curSp = stream.Read<int>();
		curHp = stream.Read<int>();
	}

	[Broadcast]
	public void Hurt(int damage)
	{
		curHp -= damage;
		if (curHp < 0)
		{
			Scene.Components.Get<Chat>( FindMode.InDescendants ).AddText( "", "Player killed" );
		}
	}
}
