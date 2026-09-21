using System.Collections.Generic;

namespace Repository;

public class Scenes : IRepository<SceneId> {
    private static readonly Dictionary<SceneId, string> _sceneUid = new() {
        { SceneId.MainLevel, "uid://bqhfk8hfk4s4c" }
    };

    public static string ValidateUids() => RepositoryValidator.Validate(_sceneUid, nameof(Scenes));

    public static string GetUid(SceneId id) {
        return _sceneUid[id];
    }
}

public enum SceneId {
    MainLevel
}