using System;
using System.Linq;
using Godot;

public partial class Player : Node2D {
    #region PlayerState

    public enum State {
        Jumping,
        Falling,
        Grounded
    }

    #endregion

    #region Exports

    [Export]
    private AnimatedSprite2D _animatedSprite2D;

    [Export]
    private ShapeCast2D _groundedShapeCast;

    #endregion

    private PlayerStats _stats = new() { MoveSpeed = 100, JumpSpeed = 200, JumpDuration = .25f, Gravity = 400, MaxFallSpeed = 300 };
    private Vector2 _velocity = Vector2.Zero;
    private State _state = State.Falling;
    private float _jumpTimeLeft = 0;

    public override void _Ready() {
        KeyBind.Initialize(); // TODO: Refactor and move elsewhere
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

        if (Input.IsActionJustPressed(KeyBind.Jump)) {
            _velocity.Y = -_stats.JumpSpeed;
            _jumpTimeLeft = _stats.JumpDuration;
        }
    }

    private void _DetermineState(double delta) {
        if (_state == State.Jumping) {
            _jumpTimeLeft -= (float)delta;
            if (_jumpTimeLeft <= 0) {
                _state = State.Falling;
            }
        }

        if (_state is State.Falling or State.Grounded) {
            bool grounded = Enumerable
                .Range(0, _groundedShapeCast.GetCollisionCount())
                .Any(i => _groundedShapeCast.GetCollider(i) is StaticBody2D);

            _state = grounded ? State.Grounded : State.Falling;
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
        _animatedSprite2D.GlobalPosition += _velocity * (float)delta;
    }
}

public record struct PlayerStats {
    public float MoveSpeed { get; init; }
    public float JumpSpeed { get; init; }
    public float JumpDuration { get; init; }
    public float Gravity { get; init; }
    public float MaxFallSpeed { get; init; }
}