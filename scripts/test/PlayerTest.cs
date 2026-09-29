using Godot;

public partial class PlayerTest : Node {
    #region Exports

    [Export]
    private Player _player;

    [Export]
    private Button _damagePlayer;

    [Export]
    private Button _healPlayer;

    [Export]
    private Button _killPlayer;

    [Export]
    private Button _enterDoor;

    #endregion

    public override void _Ready() {
        _damagePlayer.Pressed += _DamagePlayer;
        _healPlayer.Pressed += _HealPlayer;
        _killPlayer.Pressed += _KillPlayer;
        _enterDoor.Pressed += _EnterDoor;
    }

    private void _KillPlayer() {
        _player.Kill();
    }

    private void _HealPlayer() {
        _player.Heal(1);
    }

    private void _DamagePlayer() {
        _player.Damage(1);
    }

    private void _EnterDoor() {
        _player.EnterDoor();
    }
}