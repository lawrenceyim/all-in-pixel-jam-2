using AddOns.Repository;
using Godot;

public partial class Door : Area2D, IInteractable {
    [Export]
    private Vector2 _spawnPosition;

    [Export]
    private SceneId _leadsTo;

    [Export]
    private KeyCard.Color _color;

    [Export]
    private AudioStreamPlayer2D _sfxPlayer;

    [Export]
    private AudioStream _unlockingSfx;

    public void Interact(InteractionContext interactionContext) {
        if (interactionContext is not PlayerInteractionContext playerContext) {
            GD.PrintErr("Interaction context is not PlayerInteractionContext in Door");
            return;
        }

        // TODO: if unlocked
        if (PlayerData.DoorsUnlocked.Contains(_color)) {
            playerContext.Player.DoorEntered += () => {
                Callable.From(() => {
                    {
                        _ = EventManager.MoveScene(_leadsTo, _spawnPosition);
                    }
                }).CallDeferred();
            };
            playerContext?.Player.EnterDoor();
            return;
        }

        if (!PlayerData.CardsFound.Contains(_color)) {
            return;
        }

        // unlocking sfx
        PlayerData.DoorsUnlocked.Add(_color);
        _sfxPlayer.Stream = _unlockingSfx;
        _sfxPlayer.Play();
    }
}