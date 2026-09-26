using AddOns.Repository;
using Godot;

public partial class KeyCard : Area2D, IInteractable {
    public enum Color {
        Blue,
        Green,
        Red,
    }

    [Export]
    private Color _color;

    [Export]
    private Sprite2D _sprite;

    public void SetColor(Color color) {
        _color = color;
        string uid = color switch {
            Color.Blue => Textures.GetUid(TextureId.BlueKeyCard),
            Color.Green => Textures.GetUid(TextureId.GreenKeyCard),
            Color.Red => Textures.GetUid(TextureId.RedKeyCard)
        };
        _sprite.Texture = ResourceLoader.Load<Texture2D>(uid);
    }

    public void Interact() {
        GD.Print($"Keycard {_color} obtained");
        PlayerData.CardsFound.Add(_color);
        QueueFree();
    }
}