#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using AddOns.Repository;
using Godot;

public static class EventManager {
    private static SceneTree? _sceneTree;
    private static SceneTree SceneTree => _sceneTree ??= (SceneTree)Engine.GetMainLoop();

    public static async Task MoveScene(SceneId sceneId, Vector2 playerPosition) {
        SceneManager.Unload();
        Node scene = await SceneManager.LoadScene(sceneId);
        GameObjectManager.Add(SceneTree.Root, GameObjects.GetUid(GameObjectId.Player), new SpawnOptions { Position = playerPosition });
        List<ISceneCommand> commands = SpawnCommands.GetCommands(sceneId);
        GD.Print($"EventManager MoveScene {sceneId} Spawn Command count {commands.Count}");
        foreach (ISceneCommand command in commands) {
            GD.Print(command.ToString());
            command.Execute(scene);
        }
    }
}