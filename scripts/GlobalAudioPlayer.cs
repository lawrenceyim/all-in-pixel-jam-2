using System.Threading.Tasks;
using Godot;

public class GlobalAudioPlayer {
    private static string _songUid = "uid://c4ww7tyor5p2f";
    private static AudioStreamPlayer _songPlayer;
    private static Task<AudioStreamPlayer> _songPlayerTask;

    public static async Task PlaySong() {
        _songPlayerTask ??= _AddAudioStreamPlayer();
        _songPlayer = await _songPlayerTask;
        if (_songPlayer.IsPlaying()) {
            return;
        }

        // Loop enabled in the stream itself in editor
        _songPlayer.VolumeDb = -40;
        _songPlayer.Stream = GD.Load<AudioStream>(_songUid);
        _songPlayer.Play();
    }

    public void StopPlaying() {
        _songPlayer.Stop();
    }

    private static async Task<AudioStreamPlayer> _AddAudioStreamPlayer() {
        SceneTree tree = (SceneTree)Engine.GetMainLoop();
        Window root = tree.Root;

        if (!root.IsNodeReady()) {
            await root.ToSignal(root, Node.SignalName.Ready);
        }

        AudioStreamPlayer streamPlayer = new AudioStreamPlayer();
        tree.Root.AddChild(streamPlayer);
        return streamPlayer;
    }
}