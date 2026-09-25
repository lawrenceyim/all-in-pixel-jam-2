using Godot;

public partial class KeyCard : Area2D, IInteractable {
    public enum Color {
        Blue,
        Green,
        Red,
    }

    [Export]
    private Color _color;

    public void SetColor(Color color) {
        // TODO: Change texture
    }

    public void Interact() {
        GD.Print($"Keycard {_color} obtained");
        PlayerData.CardsFound.Add(_color);
        QueueFree();
    }
}