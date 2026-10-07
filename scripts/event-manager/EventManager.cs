#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using AddOns.Repository;
using Godot;

public static class EventManager {
    private static SceneTree? _sceneTree;
    private static SceneTree SceneTree => _sceneTree ??= (SceneTree)Engine.GetMainLoop();

    public static async Task MoveScene(SceneId sceneId, SpawnOptions spawnOptions) {
        SceneTree tree = (SceneTree)Engine.GetMainLoop();
        await tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame);
        tree.UnloadCurrentScene();
        Node2D scene = new() { Name = sceneId.ToString() };
        tree.Root.AddChild(scene);
        tree.CurrentScene = scene;
        GameObjectManager.Add(scene, GameObjects.GetUid(GameObjectId.Player), spawnOptions);

        Dictionary<Vector2, TileId> tiles = RoomTiles.GetTiles(sceneId);
        TileMapLayer tileMap = TileMapFactory.CreateTileMap(tiles);
        scene.AddChild(tileMap);

        Doors.SpawnDoors(scene, sceneId);


        List<ISceneCommand> commands = SpawnCommands.GetCommands(sceneId);
        // GD.Print($"EventManager MoveScene {sceneId} Spawn Command count {commands.Count}");
        foreach (ISceneCommand command in commands) {
            GD.Print(command.ToString());
            command.Execute(scene);
        }
    }

    public static async Task ChangeToEnd() {
        SceneTree tree = (SceneTree)Engine.GetMainLoop();
        await tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame);
        tree.UnloadCurrentScene();
        tree.ChangeSceneToFile(Scenes.GetUid(SceneId.End));
    }
}