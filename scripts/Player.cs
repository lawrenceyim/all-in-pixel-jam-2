#nullable enable
using System;
using System.Linq;
using Godot;

public partial class Player : Node2D {
    #region Enums

    private enum State {
        Jumping,
        Falling,
        Grounded
    }

    private enum Sfx {
        Jump,
        Swing
    }

    #endregion

    #region Exports

    [Export]
    private AnimatedSprite2D _playerSprite;

    [Export]
    private ShapeCast2D _groundedShapeCast;

    [Export]
    private AudioStreamPlayer2D _sfxPlayer;

    [Export]
    private AudioStream _swingSfx;

    [Export]
    private AudioStream _jumpSfx;

    [Export]
    private Area2D _interactionHitbox;

    #endregion

    private PlayerStats _stats = new() { MoveSpeed = 100, JumpSpeed = 200, JumpDuration = .25f, Gravity = 400, MaxFallSpeed = 300, NumberOfJumps = 1 };
    private Vector2 _velocity = Vector2.Zero;
    private State _state = State.Falling;
    private float _jumpTimeLeft = 0;
    private IInteractable? _interactable;
    private int _jumpsLeft = 1;

    public override void _Ready() {
        KeyBind.Initialize(); // TODO: Refactor and move elsewhere
        _interactionHitbox.AreaEntered += _SetInteractable;
        _interactionHitbox.AreaExited += _ClearInteractable;
    }

    public override void _Process(double delta) {
        _DetermineState(delta);
        _Input(delta);
        _Gravity(delta);
        _Move(delta);
    }

    private void _Input(double delta) {
        float xVelocity = 0;
        if (Input.IsActionPressed(KeyBind.MoveLeft)) {
            xVelocity -= 1;
        }

        if (Input.IsActionPressed(KeyBind.MoveRight)) {
            xVelocity += 1;
        }

        _velocity.X = xVelocity * _stats.MoveSpeed;

        if (Input.IsActionJustPressed(KeyBind.Jump) && _jumpsLeft > 0) {
            _PlaySfx(Sfx.Jump);
            _state = State.Jumping;
            _velocity.Y = -_stats.JumpSpeed;
            _jumpTimeLeft = _stats.JumpDuration;
            _jumpsLeft--;
            GD.Print($"Jumped. State is {_state}. Jump count left {_jumpsLeft}");
        }

        if (Input.IsActionJustPressed(KeyBind.Action)) {
            GD.Print($"Action pressed.");
            _Interact();
        }
    }

    private void _DetermineState(double delta) {
        if (_state == State.Jumping) {
            _jumpTimeLeft -= (float)delta;
            if (_jumpTimeLeft <= 0) {
                _state = State.Falling;
            }

            return;
        }

        if (_state is State.Falling or State.Grounded) {
            bool grounded = Enumerable
                .Range(0, _groundedShapeCast.GetCollisionCount())
                .Any(i => _groundedShapeCast.GetCollider(i) is StaticBody2D);

            _state = grounded ? State.Grounded : State.Falling;

            if (_state is State.Grounded) {
                _jumpsLeft = _stats.NumberOfJumps;
            }
        }
    }

    private void _Gravity(double delta) {
        if (_state != State.Falling) {
            _velocity.Y = Math.Min(_velocity.Y, 0);
            return;
        }

        _velocity.Y += _stats.Gravity * (float)delta;
        _velocity.Y = Math.Min(_velocity.Y, _stats.MaxFallSpeed);
    }

    private void _Move(double delta) {
        _playerSprite.GlobalPosition += _velocity * (float)delta;
    }

    private void _PlaySfx(Sfx sfx) {
        switch (sfx) {
            case Sfx.Jump:
                _sfxPlayer.Stream = _jumpSfx;
                _sfxPlayer.Play();
                break;
        }
    }

    private void _Interact() {
        _interactable?.Interact();
    }

    private void _ClearInteractable(Area2D area) {
        if (area is IInteractable interactable && interactable == _interactable) {
            _interactable = null;
        }
    }

    private void _ClearInteractable() {
        GD.Print("Cleared Interactable");
        _interactable = null;
    }

    private void _SetInteractable(Area2D area) {
        if (area is IInteractable interactable) {
            GD.Print($"Set Interactable {interactable}");
            _interactable = interactable;
        }
    }
}

public record struct PlayerStats {
    public float MoveSpeed { get; init; }
    public float JumpSpeed { get; init; }
    public float JumpDuration { get; init; }
    public int NumberOfJumps { get; init; }
    public float Gravity { get; init; }
    public float MaxFallSpeed { get; init; }
}