using Godot;

public partial class KeyCard : Area2D, IInteractable {
    public enum Color {
        Blue,
        Green,
        Red,
    }

    [Export]
    private Color _color;

    public void Interact() {
        PlayerData.CardsFound.Add(_color);
        QueueFree();
    }
}