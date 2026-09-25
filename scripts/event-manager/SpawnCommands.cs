using System.Collections.Generic;
using AddOns.Repository;
using Godot;

public static class SpawnCommands {
    private static readonly Dictionary<SceneId, List<ISceneCommand>> _commands = new() {
        {
            SceneId.MainLevel, []
        }, {
            SceneId.LevelOne, [
                new SpawnKeyCard(new Vector2(100, 100), KeyCard.Color.Blue, () => !PlayerData.CardsFound.Contains(KeyCard.Color.Blue))
            ]
        },
    };

    public static List<ISceneCommand> GetCommands(SceneId id) {
        return _commands[id];
    }
}