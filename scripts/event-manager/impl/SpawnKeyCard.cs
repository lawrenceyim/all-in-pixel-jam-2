using System;
using AddOns.Repository;
using Godot;

public record SpawnKeyCard(Vector2 Position, KeyCard.Color Color, Func<bool> SpawnCondition) : ISceneCommand {
    public void Execute(Node scene) {
        if (!SpawnCondition()) {
            return;
        }

        string uid = GameObjects.GetUid(GameObjectId.KeyCard);
        Node added = GameObjectManager.Add(scene, uid, new SpawnOptions { Position = Position });
        KeyCard keyCard = added as KeyCard;
        keyCard?.SetColor(Color);
    }
}