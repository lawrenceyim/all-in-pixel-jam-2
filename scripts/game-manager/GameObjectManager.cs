using Godot;

public static class GameObjectManager {
    public static void Add(Node parent, string sceneName, SpawnOptions spawnOptions) {
        PackedScene packedScene = GD.Load<PackedScene>(sceneName);
        Node node = packedScene.Instantiate();
        parent.AddChild(node);

        if (spawnOptions.Position is { } position && node is Node2D node2D) {
            node2D.GlobalPosition = position;
        }
    }
}