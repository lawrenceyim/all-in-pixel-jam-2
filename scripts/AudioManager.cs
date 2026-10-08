using System.Collections.Generic;
using Godot;

public static class AudioManager {
    public enum AudioType {
        Music,
        Sfx
    }

    private static List<AudioPlayer> _sfxPlayers = [];
    private static List<AudioPlayer> _musicPlayers = [];
    public static int SfxVolume = -25;
    public static int MusicVolume = -25;

    public static void SetSfxVolume(int volume) {
        SfxVolume = volume;
        foreach (AudioPlayer audioPlayer in _sfxPlayers) {
            audioPlayer.SetVolume(volume);
        }
    }

    public static void SetMusicVolume(int volume) {
        MusicVolume = volume;
        foreach (AudioPlayer audioPlayer in _musicPlayers) {
            audioPlayer.SetVolume(volume);
        }
    }

    public static void AddAudioStreamPlayer(AudioPlayer audioPlayer, AudioType audioType = AudioType.Sfx) {
        switch (audioType) {
            case AudioType.Music:
                _musicPlayers.Add(audioPlayer);
                audioPlayer.SetVolume(MusicVolume);
                break;
            case AudioType.Sfx:
                _sfxPlayers.Add(audioPlayer);
                audioPlayer.SetVolume(SfxVolume);
                break;
        }
    }

    public static void RemoveAudioStreamPlayer(AudioPlayer audioPlayer, AudioType audioType = AudioType.Sfx) {
        switch (audioType) {
            case AudioType.Music:
                _musicPlayers.Remove(audioPlayer);
                break;
            case AudioType.Sfx:
                _sfxPlayers.Remove(audioPlayer);
                break;
        }
    }
}

public abstract record AudioPlayer {
    public abstract void SetVolume(int volume);
}

public record AudioPlayer2DDto(AudioStreamPlayer2D AudioStreamPlayer) : AudioPlayer {
    public override void SetVolume(int volume) {
        AudioStreamPlayer.VolumeDb = volume;
    }
}

public record AudioPlayerDto(AudioStreamPlayer AudioStreamPlayer) : AudioPlayer {
    public override void SetVolume(int volume) {
        AudioStreamPlayer.VolumeDb = volume;
    }
}