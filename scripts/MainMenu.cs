using Godot;
using System;
using AddOns.Repository;

public partial class MainMenu : Node {
    public override void _Ready() {
        _StartGame();
    }

    private void _StartGame() {
        Callable.From(() => {
            _ = EventManager.MoveScene(SceneId.B, new SpawnOptions {
                Position = new Vector2(130, -30)
            });
        }).CallDeferred();
    }
}