using Sandbox;
using System.ComponentModel;
using System.Reflection;

public sealed class AnimationPropertyComponent : BaseComponent
{
	[Property] public GameObject animated { get; set; }

	[Property] BaseComponent component { get; set; }

	[Property] CitizenAnimation animation { get; set; }

	[Property] string componentName { get; set; }

	[Property] AnimTypes type { get; set; }
	enum AnimTypes
	{
		Property,
		Method
	}

	[Property] string propertyName { get; set; }

	[Property] string methodName { get; set; }

	TypeDescription componentAnimated { get; set; }

	protected override void OnStart()
	{
		base.OnStart();
		componentAnimated = TypeLibrary.GetType( componentName );
		Type a = componentAnimated.GetType();
		if (type == AnimTypes.Property)
		{
			
		}
		
	}
	protected override void OnUpdate()
	{
		
	}
}
