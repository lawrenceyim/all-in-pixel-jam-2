using Godot;

public partial class Player : Node2D {
    [Export]
    private AnimatedSprite2D _animatedSprite2D;

    private PlayerStats _stats = new() { MoveSpeed = 100, JumpSpeed = 100 };

    public override void _Ready() {
        KeyBind.Initialize(); // TODO: Refactor and move elseweher
    }

    public override void _Process(double delta) {
        _Move(delta);
    }

    private void _Move(double delta) {
        Vector2 velocity = Vector2.Zero;
        if (Input.IsActionPressed(KeyBind.MoveLeft)) {
            velocity.X -= 1;
        }

        if (Input.IsActionPressed(KeyBind.MoveRight)) {
            velocity.X += 1;
        }

        _animatedSprite2D.GlobalPosition += velocity * _stats.MoveSpeed * (float)delta;
    }
}

public record struct PlayerStats {
    public float MoveSpeed { get; init; }
    public float JumpSpeed { get; init; }
}