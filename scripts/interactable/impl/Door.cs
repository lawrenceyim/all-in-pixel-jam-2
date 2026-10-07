using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddOns.Repository;
using Godot;

public partial class Door : Area2D, IInteractable {
    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private AudioStreamPlayer2D _sfxPlayer;

    [Export]
    private AudioStream _unlockingSfx;

    [Export]
    private AudioStream _openingSfx;

    [Export]
    private Sprite2D _redIcon;

    [Export]
    private Sprite2D _greenIcon;

    [Export]
    private Sprite2D _blueIcon;

    [Export]
    private Texture2D _dimIconTexture;

    [Export]
    private Doors.DoorId _id;

    private List<KeyCard.Color> _colors = [];

    private enum Animation {
        Open,
        Close
    }

    private const string Open = "open";
    private const string Close = "close";

    public override void _Ready() { }

    public void Init(Doors.DoorId id, List<KeyCard.Color> colors) {
        _id = id;
        _colors = colors;
        _DimColorIcons();
    }

    public void Interact(InteractionContext interactionContext) {
        if (interactionContext is not PlayerInteractionContext playerContext) {
            GD.PrintErr("Interaction context is not PlayerInteractionContext in Door");
            return;
        }

        if (_HaveAllCards()) {
            playerContext.Player.DoorEntered += async () => {
                Task animationTask = _WaitForSignal(_sprite, AnimatedSprite2D.SignalName.AnimationFinished);
                Task audioTask = _WaitForSignal(_sfxPlayer, AudioStreamPlayer.SignalName.Finished);
                _PlayAnimation(Animation.Open);
                _sfxPlayer.Stream = _openingSfx;
                _sfxPlayer.Play();

                await Task.WhenAll(
                    animationTask,
                    audioTask
                );

                Callable.From(() => {
                    {
                        if (_id == Doors.DoorId.Goal) {
                            _ = EventManager.ChangeToEnd();
                            return;
                        }

                        Doors.DoorId destinationDoorId = Doors.GetDestinationDoorId(_id);
                        Vector2 position = Doors.DoorPosition(Doors.GetDoorLocation(destinationDoorId));
                        SceneId leadsTo = Doors.GetDestinationScene(_id);
                        GD.Print($"Entering scene {leadsTo}");
                        _ = EventManager.MoveScene(leadsTo, new SpawnOptions { Position = position });
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

    private void _DimColorIcons() {
        if (!_colors.Contains(KeyCard.Color.Red)) {
            _redIcon.Texture = _dimIconTexture;
        }

        if (!_colors.Contains(KeyCard.Color.Green)) {
            _greenIcon.Texture = _dimIconTexture;
        }

        if (!_colors.Contains(KeyCard.Color.Blue)) {
            _blueIcon.Texture = _dimIconTexture;
        }
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