using System.Collections.Generic;

namespace AddOns.Repository;

public class Scenes : IRepository<SceneId> {
    private static readonly Dictionary<SceneId, string> _sceneUid = new() {
        { SceneId.Tutorial, "uid://dfm8mvk6vjyoh" },
        { SceneId.End, "uid://chh3nh5k7euk8" }
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
    Goal,
    Tutorial,
    End
}