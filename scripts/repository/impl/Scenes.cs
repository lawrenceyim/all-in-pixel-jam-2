using System.Collections.Generic;

namespace AddOns.Repository;

public class Scenes : IRepository<SceneId> {
    private static readonly Dictionary<SceneId, string> _sceneUid = new() {
        { SceneId.A, "uid://d2s15qnsdfpl" },
        { SceneId.B, "uid://bqhfk8hfk4s4c" },
    };

    public static string ValidateUids() => RepositoryValidator.Validate(_sceneUid, nameof(Scenes));

    public static string GetUid(SceneId id) {
        return _sceneUid[id];
    }
}

public enum SceneId {
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    M,
    N,
    O,
    Goal
}