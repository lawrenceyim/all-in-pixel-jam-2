#nullable enable
using Godot;
using AddOns.Repository;

public static class SceneManager {
    private static SceneTree? _sceneTree;
    private static SceneTree SceneTree => _sceneTree ??= (SceneTree)Engine.GetMainLoop();

    public static void LoadScene(SceneId sceneId) {
        string uid = Scenes.GetUid(sceneId);
        _LoadScene(uid);
    }

    private static void _LoadScene(string sceneName) {
        SceneTree.ChangeSceneToFile(sceneName);
    }

    public static void Unload() {
        SceneTree.UnloadCurrentScene();
    }
}