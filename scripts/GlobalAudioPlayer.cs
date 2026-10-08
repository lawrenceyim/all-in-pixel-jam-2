using System.Threading.Tasks;
using Godot;

public class GlobalAudioPlayer {
    public enum SongId {
        None,
        Title,
        Gameplay,
        Ending
    }

    private static string _songUid = "uid://c4ww7tyor5p2f";
    private static string _titleSongUid = "uid://b4ca12l4wkmtj";
    private static string _gameplaySongUid = "uid://cavcqkub3ffvw";
    private static string _endingSongUid = "uid://cox3e7c3nvjcm";
    private static AudioStreamPlayer _songPlayer;
    private static AudioPlayer _songPlayerDto;
    private static Task<AudioStreamPlayer> _songPlayerTask;
    private static SongId _songId = SongId.None;

    public static async Task PlaySong() {
        _songPlayerTask ??= _AddAudioStreamPlayer();
        _songPlayer = await _songPlayerTask;
        _songPlayerDto = new AudioPlayerDto(_songPlayer);
        AudioManager.AddAudioStreamPlayer(_songPlayerDto, AudioManager.AudioType.Music);
        if (_songPlayer.IsPlaying()) {
            return;
        }

        // Loop enabled in the stream itself in editor
        _songPlayer.Stream = GD.Load<AudioStream>(_songUid);
        _songPlayer.Play();
    }

    public static async Task PlaySong(SongId id) {
        _songPlayerTask ??= _AddAudioStreamPlayer();
        _songPlayer = await _songPlayerTask;
        _songPlayerDto = new AudioPlayerDto(_songPlayer);
        AudioManager.AddAudioStreamPlayer(_songPlayerDto, AudioManager.AudioType.Music);

        if (_songId == id) {
            return;
        }

        string uid = id switch {
            SongId.Title => _titleSongUid,
            SongId.Gameplay => _gameplaySongUid,
            SongId.Ending => _endingSongUid,
            _ => "",
        };

        // Loop enabled in the stream itself in editor
        _songPlayer.Stream = GD.Load<AudioStream>(uid);
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

        AudioStreamPlayer streamPlayer = new();
        tree.Root.AddChild(streamPlayer);

        return streamPlayer;
    }
}