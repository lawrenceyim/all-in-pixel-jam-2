using Godot;

public static class GameObjectManager {
    public static Node Add(Node parent, string sceneName, SpawnOptions spawnOptions) {
        GD.Print($"GameObjectManager::Add({sceneName}, {spawnOptions})");
        PackedScene packedScene = GD.Load<PackedScene>(sceneName);
        Node node = packedScene.Instantiate();
        parent.AddChild(node);

        // Remove this and let the caller do this?
        if (spawnOptions.Position is { } position && node is Node2D node2D) {
            node2D.GlobalPosition = position;
        }

        return node;
    }
}