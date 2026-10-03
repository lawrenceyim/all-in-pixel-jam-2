#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public partial class Player : Node2D {
    #region Events

    public event Action DoorEntered;

    #endregion

    #region Enums

    private enum State {
        Jumping,
        Falling,
        Grounded,
        EnteringDoor
    }

    private enum Sfx {
        Jump,
        Swing
    }

    private enum Animation {
        Walk,
        Jump,
        Idle,
        EnteringDoor,
    }

    #endregion

    #region Animation Names

    private const string Walk = "walk";
    private const string Jump = "jump";
    private const string Idle = "idle";
    private const string EnteringDoor = "enter_door";

    #endregion

    #region Exports

    [Export]
    private AnimatedSprite2D _playerSprite;

    [Export]
    private ShapeCast2D _groundedShapeCast;

    [Export]
    private AudioStreamPlayer2D _sfxPlayer;

    [Export]
    private AudioStream _swingSfx;

    [Export]
    private AudioStream _jumpSfx;

    [Export]
    private Area2D _interactionHitbox;

    [Export]
    private HealthDisplay _healthDisplay;

    #endregion

    private PlayerStats _stats = new() { MoveSpeed = 100, JumpSpeed = 100, JumpDuration = .5f, Gravity = 400, MaxFallSpeed = 300, NumberOfJumps = 1 };
    private Vector2 _velocity = Vector2.Zero;
    private double _jumpTimeLeft = 0;
    private IInteractable? _interactable;
    private int _jumpsLeft = 1;

    private Dictionary<State, IPlayerState> _states = new();
    private IPlayerState _state;

    public override void _Ready() {
        KeyBind.Initialize(); // TODO: Refactor and move elsewhere
        _interactionHitbox.AreaEntered += _SetInteractable;
        _interactionHitbox.AreaExited += _ClearInteractable;
        _SetHealth(PlayerData.Health);

        _states[State.Grounded] = new GroundedState(this);
        _states[State.Falling] = new FallingState(this);
        _states[State.Jumping] = new JumpingState(this);
        _states[State.EnteringDoor] = new EnteringDoorState(this);
        _ChangeState(State.Grounded);
        _playerSprite.Play();
    }

    public override void _Process(double delta) {
        _state?.Process(delta);
    }

    public override void _PhysicsProcess(double delta) {
        _state?.PhysicsProcess(delta);
    }

    public void Damage(int damage) {
        int health = Math.Max((int)PlayerData.Health - damage, 0);
        _SetHealth((HealthDisplay.HealthAmount)health);
        GD.Print($"Player damaged by {damage} to {PlayerData.Health}");
        // TODO: Hurt sfx
    }

    public void Heal(int heal) {
        int health = Math.Min((int)PlayerData.Health + heal, (int)HealthDisplay.HealthAmount.Four);
        _SetHealth((HealthDisplay.HealthAmount)health);
        GD.Print($"Player healed by {heal} to {PlayerData.Health}");
        // TODO: Heal sfx
    }

    public void Kill() {
        _SetHealth(HealthDisplay.HealthAmount.Zero);
        GD.Print("Player Killed");

        // TODO: Set animation to death
        // Emit signal to alert game to restart after animation finishes?
    }

    public void EnterDoor() {
        _ChangeState(State.EnteringDoor);
    }

    private void _ClearInteractable(Area2D area) {
        if (area is IInteractable interactable && interactable == _interactable) {
            _interactable = null;
        }
    }

    private void _ClearInteractable() {
        GD.Print("Cleared Interactable");
        _interactable = null;
    }

    private void _SetInteractable(Area2D area) {
        if (area is IInteractable interactable) {
            GD.Print($"Set Interactable {interactable}");
            _interactable = interactable;
        }
    }

    private void _SetHealth(HealthDisplay.HealthAmount health) {
        PlayerData.Health = health;
        _healthDisplay.SetHealth(health);
    }

    #region Shared functions for states

    private void _ChangeState(State state) {
        _state?.Exit();
        _state = _states[state];
        _state?.Enter();
    }

    private void _PlaySfx(Sfx sfx) {
        switch (sfx) {
            case Sfx.Jump:
                _sfxPlayer.Stream = _jumpSfx;
                _sfxPlayer.Play();
                break;
        }
    }

    private void _Interact() {
        _interactable?.Interact(new PlayerInteractionContext(this));
    }

    private void _Move(double delta) {
        _playerSprite.GlobalPosition += _velocity * (float)delta;
    }

    private bool _IsGrounded() {
        return Enumerable
            .Range(0, _groundedShapeCast.GetCollisionCount())
            .Any(i => _groundedShapeCast.GetCollider(i) is StaticBody2D);
    }

    private void HorizontalMovementInput() {
        float xVelocity = 0;
        if (Input.IsActionPressed(KeyBind.MoveLeft)) {
            xVelocity -= 1;
        }

        if (Input.IsActionPressed(KeyBind.MoveRight)) {
            xVelocity += 1;
        }

        _velocity.X = xVelocity * _stats.MoveSpeed;
    }

    private void _JumpInput() {
        if (!Input.IsActionJustPressed(KeyBind.Jump) || _jumpsLeft <= 0) {
            return;
        }

        _ChangeState(State.Jumping);
    }

    private void _InteractionInput() {
        if (Input.IsActionJustPressed(KeyBind.Action)) {
            GD.Print($"Action pressed.");
            _Interact();
        }
    }

    private void _Gravity(double delta) {
        _velocity.Y += _stats.Gravity * (float)delta;
        _velocity.Y = Math.Min(_velocity.Y, _stats.MaxFallSpeed);
    }

    private void _FlipPlayerSprite() {
        if (Mathf.IsZeroApprox(_velocity.X)) {
            return;
        }

        _playerSprite.FlipH = _velocity.X < 0;
    }

    private void _SetAnimation(Animation animation) {
        switch (animation) {
            case Animation.Walk:
                // _playerSprite.SpriteFrames.SetAnimationLoopMode(Walk, SpriteFrames.LoopMode.Linear);
                _playerSprite.Play(Walk);
                break;
            case Animation.Idle:
                _playerSprite.Play(Idle);
                break;
            case Animation.EnteringDoor:
                _playerSprite.Play(EnteringDoor);
                break;
            case Animation.Jump:
                _playerSprite.Play(Jump);
                break;
        }
    }

    #endregion

    #region States

    private interface IPlayerState {
        public State Id();
        public void Enter();
        public void Exit();
        public void Process(double delta);
        public void PhysicsProcess(double delta);
    }

    private sealed class GroundedState(Player player) : IPlayerState {
        public State Id() => State.Grounded;

        public void Enter() {
            player._jumpsLeft = player._stats.NumberOfJumps;
        }

        public void Exit() { }

        public void Process(double delta) {
            player.HorizontalMovementInput();
            player._JumpInput();
            player._InteractionInput(); // Must be kept at end or else it causes edge case where door animation is started but other code in Process override and soft locks player
            // alternative approach: use this guard clause
            if (player._state != this) {
                return;
            }

            player._FlipPlayerSprite();
            player._SetAnimation(!player._velocity.IsZeroApprox() ? Animation.Walk : Animation.Idle);
        }

        public void PhysicsProcess(double delta) {
            if (!player._IsGrounded()) {
                player._ChangeState(State.Falling);
            }

            player._Move(delta);
        }
    }

    private sealed class FallingState(Player player) : IPlayerState {
        public State Id() => State.Falling;

        public void Enter() {
            player._SetAnimation(Animation.Idle);
            // TODO: Set falling animation
        }

        public void Exit() {
            player._velocity.Y = 0;
        }

        public void Process(double delta) {
            if (player._IsGrounded()) {
                player._ChangeState(State.Grounded);
                return;
            }

            player._Gravity(delta);
            player.HorizontalMovementInput();
            player._JumpInput();
            player._InteractionInput();
            if (player._state != this) {
                return;
            }

            player._FlipPlayerSprite();
            // TODO: Set animation
        }

        public void PhysicsProcess(double delta) {
            player._Move(delta);
        }
    }

    private sealed class JumpingState(Player player) : IPlayerState {
        public State Id() => State.Jumping;

        public void Enter() {
            player._PlaySfx(Sfx.Jump);
            player._SetAnimation(Animation.Jump);
            player._velocity.Y = -player._stats.JumpSpeed;
            player._jumpTimeLeft = player._stats.JumpDuration;
            player._jumpsLeft--;
            GD.Print($"Jumped. Jump count left {player._jumpsLeft}");
        }

        public void Exit() { }

        public void Process(double delta) {
            player.HorizontalMovementInput();
            player._JumpInput();
            player._FlipPlayerSprite();
            // TODO: Set animation
            player._InteractionInput();
        }

        public void PhysicsProcess(double delta) {
            player._Move(delta);
            player._jumpTimeLeft -= delta;
            if (player._jumpTimeLeft <= 0) {
                player._ChangeState(State.Falling);
            }
        }
    }

    private sealed class EnteringDoorState(Player player) : IPlayerState {
        public State Id() => State.EnteringDoor;
        private bool _active = false;

        public async void Enter() {
            try {
                _active = true;
                GD.Print("Player entered EnteringDoor state");

                Task animationFinished = _WaitForSignal(player._playerSprite, AnimatedSprite2D.SignalName.AnimationFinished);
                // Task audioFinished = _WaitForSignal(player._sfxPlayer, AudioStreamPlayer2D.SignalName.Finished);

                player._SetAnimation(Animation.EnteringDoor);
                // player._sfxPlayer.Play(); // TODO: create function to play a specific SFX

                await Task.WhenAll(
                    // audioFinished,
                    animationFinished
                );

                if (!_active) {
                    // Edge case for if player is interrupted and state changes. Another animation like hurt animation shouldn't result in scene change.
                    return;
                }

                GD.Print("Entered door finished");
                player.DoorEntered?.Invoke();
            }
            catch (Exception e) {
                GD.PrintErr(e);
            }
        }

        private async Task _WaitForSignal(GodotObject source, StringName signal) {
            await player.ToSignal(source, signal);
        }

        public void Exit() {
            _active = false;
        }

        public void Process(double delta) { }
        public void PhysicsProcess(double delta) { }
    }

    #endregion
}

public record struct PlayerStats {
    public float MoveSpeed { get; init; }
    public float JumpSpeed { get; init; }
    public float JumpDuration { get; init; }
    public int NumberOfJumps { get; init; }
    public float Gravity { get; init; }
    public float MaxFallSpeed { get; init; }
}