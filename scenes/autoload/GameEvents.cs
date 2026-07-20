using Game.Component;
using Godot;

namespace Game.Autoload;

public partial class GameEvents : Node
{
	public static GameEvents Instance { get; private set; }

	[Signal]
	public delegate void BuildingPlacedEventHandler(BuildingComponent buildingComponent);


	// Notification is effectively a Ready() method in this situation. Whenever a "Notification" from the system occurs,
	// It will check to see if it is a notification that the scene has been instantiated. When it gets that notification,
	// it then sets the static Instance to the node this script is attached to, which will always be the GameEvents autoload.
	// This is necessary because it then allows other scripts to use the Instance object as a way to access the GameEvents Node
	// in order to emit any signals are defined here. It keeps code clean to have signals defined and callable from here
	// instead of defined in random scripts because it makes them locatable and doesn't unnecessarily restrict access.
    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
		{
			Instance = this;
		}
	}

	public static void EmitBuildingPlaced(BuildingComponent buildingComponent)
	{
		Instance.EmitSignal(SignalName.BuildingPlaced, buildingComponent);
	}
}
