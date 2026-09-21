using Godot;

public static class KeyBind {
    #region InputNames

    public const string MoveLeft = "move_left";
    public const string MoveRight = "move_right";
    public const string Jump = "jump";
    public const string Action = "action";
    public const string Escape = "escape";

    #endregion

    public static void Initialize() {
        _SetInput();
    }

    private static void _SetInput() {
        _AddKeyInput(MoveLeft, Key.A);
        _AddKeyInput(MoveRight, Key.D);
        _AddKeyInput(Jump, Key.Space);
        _AddKeyInput(Action, Key.E);
        _AddKeyInput(Escape, Key.Escape);
    }

    private static void _AddKeyInput(string name, Key key) {
        if (!InputMap.HasAction(name)) {
            InputMap.AddAction(name);
        }

        InputEventKey eventKey = new() {
            Keycode = key
        };

        if (!InputMap.ActionHasEvent(name, eventKey)) {
            InputMap.ActionAddEvent(name, eventKey);
        }
    }
}