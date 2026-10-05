using AddOns.Repository;
using Godot;

public partial class TutorialRoom : Node {
    [Export]
    private Area2D _area;

    public override void _Ready() {
        _area.AreaEntered += area => {
            if (area.Owner is Player) {
                Callable.From(() => {
                    _ = EventManager.MoveScene(SceneId.B, new SpawnOptions {
                        Position = new Vector2(130, -30)
                    });
                }).CallDeferred();
            }
        };
    }
}