using System;
using System.Collections.Generic;
using System.Text;
using Godot;

namespace Repository;

public class Scenes : IRepository {
    private static readonly Dictionary<SceneId, string> _sceneUid = new() {
        { SceneId.MainLevel, "uid://bqhfk8hfk4s4c" }
    };

    public static string ValidateUids() => RepositoryValidator.Validate(_sceneUid, nameof(Scenes));
}

public enum SceneId {
    MainLevel
}