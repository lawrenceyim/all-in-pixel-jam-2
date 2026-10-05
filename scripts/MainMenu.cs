using AddOns.Repository;
using Godot;

public partial class MainMenu : Node {
    public override void _Ready() {
        _StartGame();
    }

    private static void _StartGame() {
        Callable.From(() => {
            string uid = Scenes.GetUid(SceneId.Tutorial);
            SceneTree tree = (SceneTree)Engine.GetMainLoop();
            Error error = tree.ChangeSceneToFile(uid);
            if (error != Error.Ok) {
                GD.PushError($"Failed to load Tutorial: {error}");
            }
        }).CallDeferred();
    }
}