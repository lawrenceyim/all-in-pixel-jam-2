using Godot;

public partial class PauseMenu : CanvasLayer {
    [Export]
    private HSlider _musicSlider;

    [Export]
    private HSlider _sfxSlider;

    public override void _Ready() {
        _musicSlider.Value = _volumeDbToSlider(AudioManager.MusicVolume);
        _sfxSlider.Value = _volumeDbToSlider(AudioManager.SfxVolume);
        _musicSlider.ValueChanged += (double val) => AudioManager.SetMusicVolume(_sliderToVolumeDb(val));
        _sfxSlider.ValueChanged += (double val) => AudioManager.SetSfxVolume(_sliderToVolumeDb(val));
    }

    private void _ChangeVolume() { }

    private static int _sliderToVolumeDb(double sliderValue) {
        return (int)Mathf.Lerp(-50, 0, sliderValue / 100.0);
    }

    private static double _volumeDbToSlider(int volume) {
        return (volume + 50) * 2.0;
    }
}