using AddOns.Repository;
using Godot;

public partial class ValidateUids : Node {
    public override void _Ready() {
        GD.Print(GameObjects.ValidateUids());
        GD.Print(Scenes.ValidateUids());
        GD.Print(Textures.ValidateUids());
    }
}