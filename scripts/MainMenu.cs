using AddOns.Repository;
using Godot;

public partial class MainMenu : Node {
    [Export]
    private Button _startButton;

    [Export]
    private Button _exitButton;

    public override void _Ready() {
        _startButton.Pressed += _StartGame;
        _exitButton.Pressed += () => { GetTree().Quit(); };
        _ = GlobalAudioPlayer.PlaySong(GlobalAudioPlayer.SongId.Title);
        KeyBind.Initialize();
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