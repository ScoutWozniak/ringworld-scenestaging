using Sandbox;

public sealed class Health : BaseComponent, INetworkSerializable
{
	[Property] public int maxShieldPoints { get; set; } = 70;
	[Property] public int maxHealthPoints { get; set; } = 45;

	public int curSp;
	public int curHp;

	public override void OnAwake()
	{
		base.OnAwake();
		curSp = maxShieldPoints;
		curHp = maxHealthPoints;
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
}
