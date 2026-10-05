using System.Collections.Generic;
using AddOns.Repository;
using Godot;

public static class SpawnCommands {
    private static readonly Dictionary<SceneId, List<ISceneCommand>> _commands = new() {
        {
            SceneId.A, []
        }, {
            SceneId.B, [
                new SpawnKeyCard(new Vector2(130, 70), KeyCard.Color.Red, () => !PlayerData.CardsFound.Contains(KeyCard.Color.Red))
            ]
        },
    };

    public static List<ISceneCommand> GetCommands(SceneId id) {
        return _commands[id];
    }
}