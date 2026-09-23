using System.Collections.Generic;

namespace AddOns.Repository;

public class Scenes : IRepository<SceneId> {
    private static readonly Dictionary<SceneId, string> _sceneUid = new() {
        { SceneId.MainLevel, "uid://bqhfk8hfk4s4c" },
        { SceneId.LevelOne, "uid://d2s15qnsdfpl" }
    };

    public static string ValidateUids() => RepositoryValidator.Validate(_sceneUid, nameof(Scenes));

    public static string GetUid(SceneId id) {
        return _sceneUid[id];
    }
}

public enum SceneId {
    MainLevel,
    LevelOne,
}