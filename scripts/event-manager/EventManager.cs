#nullable enable
using AddOns.Repository;
using Godot;

public static class EventManager {
    private static SceneTree? _sceneTree;
    private static SceneTree SceneTree => _sceneTree ??= (SceneTree)Engine.GetMainLoop();

    public static void MoveScene(SceneId sceneId, Vector2 playerPosition) {
        SceneManager.Unload();
        SceneManager.LoadScene(sceneId);
        GameObjectManager.Add(SceneTree.Root, GameObjects.GetUid(GameObjectId.Player), new SpawnOptions { Position = playerPosition });
    }
}