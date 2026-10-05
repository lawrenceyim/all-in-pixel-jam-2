using AddOns.Repository;
using Godot;

public partial class MainMenu : Node {
    public override void _Ready() {
        _StartGame();
    }

    private void _StartGame() {
        Callable.From(async () => {
            string uid = Scenes.GetUid(SceneId.Tutorial);
            SceneTree tree = (SceneTree)Engine.GetMainLoop();
            await tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame);
            tree.UnloadCurrentScene();
            tree.ChangeSceneToFile(uid);
        }).CallDeferred();
    }
}