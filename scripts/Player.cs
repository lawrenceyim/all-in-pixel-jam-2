using System;
using Godot;

public partial class Player : Node2D {
    [Export]
    private AnimatedSprite2D _animatedSprite2D;

    private PlayerStats _stats = new() { MoveSpeed = 100, JumpSpeed = 200, Gravity = 300, MaxFallSpeed = 300 };
    private Vector2 _velocity = Vector2.Zero;

    public override void _Ready() {
        KeyBind.Initialize(); // TODO: Refactor and move elsewhere
    }

    public override void _Process(double delta) {
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
        }
    }

    private void _Gravity(double delta) {
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
    public float Gravity { get; init; }
    public float MaxFallSpeed { get; init; }
}