#nullable enable
using Godot;

public static class SceneManager {
    private static SceneTree? _sceneTree;
    private static SceneTree SceneTree => _sceneTree ??= (SceneTree)Engine.GetMainLoop();

    public static void LoadScene(string sceneName) {
        SceneTree.ChangeSceneToFile(sceneName);
    }

    public static void Unload() {
        SceneTree.UnloadCurrentScene();
    }
}