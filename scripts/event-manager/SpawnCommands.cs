using System.Collections.Generic;
using AddOns.Repository;
using Godot;

public static class SpawnCommands {
    private static readonly Dictionary<SceneId, List<ISceneCommand>> _commands = new() {
        {
            SceneId.B, [
                new SpawnKeyCard(new Vector2(130, 180), KeyCard.Color.Red, () => !PlayerData.CardsFound.Contains(KeyCard.Color.Red))
            ]
        }, {
            SceneId.F, [
                new SpawnKeyCard(new Vector2(201, 68), KeyCard.Color.Blue, () => !PlayerData.CardsFound.Contains(KeyCard.Color.Blue))
            ]
        }, {
            SceneId.M, [
                new SpawnKeyCard(new Vector2(56, 68), KeyCard.Color.Green, () => !PlayerData.CardsFound.Contains(KeyCard.Color.Green))
            ]
        },
    };

    public static List<ISceneCommand> GetCommands(SceneId id) {
        return _commands[id];
    }
}