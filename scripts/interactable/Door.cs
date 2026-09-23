using AddOns.Repository;
using Godot;

public partial class Door : Area2D, IInteractable {
    [Export]
    private Vector2 _spawnPosition;

    [Export]
    private SceneId _leadsTo;

    public void Interact() {
        Callable.From(() => {
            {
                EventManager.MoveScene(_leadsTo, _spawnPosition);
            }
        }).CallDeferred();
    }
}