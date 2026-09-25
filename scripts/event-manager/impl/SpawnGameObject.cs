using System;
using AddOns.Repository;
using Godot;

public record SpawnGameObject(GameObjectId Id, Vector2 Position, Func<bool> SpawnCondition) : ISceneCommand {
    public void Execute(Node scene) {
        if (!SpawnCondition()) {
            return;
        }

        string uid = GameObjects.GetUid(Id);
        GameObjectManager.Add(scene, uid, new SpawnOptions { Position = Position });
    }
}