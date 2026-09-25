using System.Collections.Generic;

namespace AddOns.Repository;

public class GameObjects : IRepository<GameObjectId> {
    private static readonly Dictionary<GameObjectId, string> _gameObjectUid = new() {
        { GameObjectId.Player, "uid://bv7rxsv3cd3lc" },
        { GameObjectId.KeyCard, "uid://bbevog6ngdbxw" }
    };

    public static string ValidateUids() => RepositoryValidator.Validate(_gameObjectUid, nameof(GameObjects));

    public static string GetUid(GameObjectId id) {
        return _gameObjectUid[id];
    }
}

public enum GameObjectId {
    Player = 1,
    KeyCard = 2,
}