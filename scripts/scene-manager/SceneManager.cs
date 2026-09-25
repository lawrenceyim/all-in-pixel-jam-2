#nullable enable
using System.Threading.Tasks;
using Godot;
using AddOns.Repository;

public static class SceneManager {
    private static SceneTree? _sceneTree;
    private static SceneTree SceneTree => _sceneTree ??= (SceneTree)Engine.GetMainLoop();

    public static async Task<Node> LoadScene(SceneId sceneId) {
        string uid = Scenes.GetUid(sceneId);
        return await _LoadScene(uid);
    }

    private static async Task<Node> _LoadScene(string sceneName) {
        SceneTree.ChangeSceneToFile(sceneName);
        await SceneTree.ToSignal(
            SceneTree,
            SceneTree.SignalName.SceneChanged
        );
        return SceneTree.CurrentScene;
    }

    public static void Unload() {
        SceneTree.UnloadCurrentScene();
    }
}