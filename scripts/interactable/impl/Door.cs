using System.Linq;
using System.Threading.Tasks;
using AddOns.Repository;
using Godot;
using Godot.Collections;

public partial class Door : Area2D, IInteractable {
    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private Vector2 _spawnPosition;

    [Export]
    private SceneId _leadsTo;

    [Export]
    private Array<KeyCard.Color> _colors;

    [Export]
    private AudioStreamPlayer2D _sfxPlayer;

    [Export]
    private AudioStream _unlockingSfx;

    private enum Animation {
        Open,
        Close
    }

    private const string Open = "open";
    private const string Close = "close";

    public void Interact(InteractionContext interactionContext) {
        if (interactionContext is not PlayerInteractionContext playerContext) {
            GD.PrintErr("Interaction context is not PlayerInteractionContext in Door");
            return;
        }

        if (_HaveAllCards()) {
            playerContext.Player.DoorEntered += async () => {
                Task animationTask = _WaitForSignal(_sprite, AnimatedSprite2D.SignalName.AnimationFinished);
                // Task _ = _WaitForSignal() // door SFX
                _PlayAnimation(Animation.Open);

                await Task.WhenAll(
                    animationTask
                );

                Callable.From(() => {
                    {
                        _ = EventManager.MoveScene(_leadsTo, _spawnPosition);
                    }
                }).CallDeferred();
            };

            // TODO: play entering door sfx
            playerContext?.Player.EnterDoor();
            return;
        }

        // TODO: incorrect SFX

        // if (!PlayerData.CardsFound.Contains(_color)) {
        //     return;
        // }
        //
        // // unlocking sfx
        // PlayerData.DoorsUnlocked.Add(_color);
        // _sfxPlayer.Stream = _unlockingSfx;
        // _sfxPlayer.Play();
    }

    private bool _HaveAllCards() {
        return _colors.All(color => PlayerData.CardsFound.Contains(color));
    }

    private async Task _WaitForSignal(GodotObject source, StringName signal) {
        await ToSignal(source, signal);
    }

    private void _PlayAnimation(Animation animation) {
        switch (animation) {
            case Animation.Open:
                _sprite.Play(Open);
                break;
            case Animation.Close:
                _sprite.Play(Close);
                break;
        }
    }
}