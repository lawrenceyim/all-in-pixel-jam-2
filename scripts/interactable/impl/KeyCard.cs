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

    [Export]
    private AudioStreamPlayer _sfxPlayer;

    [Export]
    private AudioStream _keyCardFoundSfx;

    public void SetColor(Color color) {
        _color = color;
        string uid = color switch {
            Color.Blue => Textures.GetUid(TextureId.BlueKeyCard),
            Color.Green => Textures.GetUid(TextureId.GreenKeyCard),
            Color.Red => Textures.GetUid(TextureId.RedKeyCard)
        };
        _sprite.Texture = ResourceLoader.Load<Texture2D>(uid);
    }

    public void Interact(InteractionContext context) {
        _sfxPlayer.Stream = _keyCardFoundSfx;
        _sfxPlayer.Play();
        PlayerInteractionContext playerContext = (PlayerInteractionContext)context;
        _ = playerContext.Player.FoundKey(this, _color);
        PlayerData.CardsFound.Add(_color);
    }

    public Sprite2D GetSprite() {
        return _sprite;
    }
}