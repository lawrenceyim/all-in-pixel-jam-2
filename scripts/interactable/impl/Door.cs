using AddOns.Repository;
using Godot;

public partial class Door : Area2D, IInteractable {
    [Export]
    private Vector2 _spawnPosition;

    [Export]
    private SceneId _leadsTo;

    public void Interact(InteractionContext interactionContext) {
        if (interactionContext is not PlayerInteractionContext playerContext) {
            GD.PrintErr("Interaction context is not PlayerInteractionContext in Door");
            return;
        }

        playerContext.Player.DoorEntered += () => {
            Callable.From(() => {
                {
                    _ = EventManager.MoveScene(_leadsTo, _spawnPosition);
                }
            }).CallDeferred();
        };
        playerContext?.Player.EnterDoor();
    }
}