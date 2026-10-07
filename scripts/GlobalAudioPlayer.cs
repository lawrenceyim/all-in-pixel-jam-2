using Godot;

public class GlobalAudioPlayer {
    private static string _songUid = "uid://c4ww7tyor5p2f";
    private static AudioStreamPlayer _songPlayer;

    public static void PlaySong() {
        _songPlayer ??= _AddAudioStreamPlayer();
        if (_songPlayer.IsPlaying()) {
            return;
        }

        _songPlayer.VolumeDb = -40;
        _songPlayer.Autoplay = true;
        _songPlayer.Stream = GD.Load<AudioStream>(_songUid);
        _songPlayer.Play();
    }

    private static AudioStreamPlayer _AddAudioStreamPlayer() {
        AudioStreamPlayer streamPlayer = new AudioStreamPlayer();
        SceneTree tree = (SceneTree)Engine.GetMainLoop();
        tree.Root.AddChild(streamPlayer);
        return streamPlayer;
    }
}