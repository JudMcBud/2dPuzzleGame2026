using System.ComponentModel;
using Game.Autoload;
using Godot;

namespace Game.Component;

public partial class BuildingComponent : Node2D
{
    [Export]
    public int BuildableRadius { get; private set; }

    public override void _Ready()
    {
        // When the building comes into existence, it emits the signal of "BuildingPlaced" through the GameEvents singleton,
        // which is listened for by the GridManager, which then calls it's own OnBuildingPlaced() method. So the flow looks
        // like this:
        // Place Building in Game -> Building is created -> Building Emits Signal -> GameEvents hears signal
        // -> GameEvents lets subscribers know -> GridManger hears signal -> GridManager executes the logic for when a
        // building is placed.
        AddToGroup(nameof(BuildingComponent));
        // Wrapping the Signal Emission in a Callable allows for .CallDeferred(), which says to run the function at the end
        // of the frame, after everything else.
        Callable.From(() => GameEvents.EmitBuildingPlaced(this)).CallDeferred();
    }

    public Vector2I GetGridCellPosition()
    {
        var gridPosition = GlobalPosition / 64;
        gridPosition = gridPosition.Floor();
        return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
    }
}
