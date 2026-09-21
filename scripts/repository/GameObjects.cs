using System.Collections.Generic;

namespace Repository;

public class GameObjects : IRepository<GameObjectId> {
    private static readonly Dictionary<GameObjectId, string> _gameObjectUid = new() {
        {
            GameObjectId.Player, "uid://bqhfk8hfk4s4c" // TODO: Replace this UID
        }
    };

    public static string ValidateUids() => RepositoryValidator.Validate(_gameObjectUid, nameof(GameObjects));

    public static string GetUid(GameObjectId id) {
        return _gameObjectUid[id];
    }
}

public enum GameObjectId {
    Player = 1
}