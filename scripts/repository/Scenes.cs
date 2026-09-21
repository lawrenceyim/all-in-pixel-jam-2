using System;
using System.Collections.Generic;
using System.Text;
using Godot;

namespace Repository;

public class Scenes : IRepository {
    private static readonly Dictionary<SceneId, string> _sceneUid = new() {
        { SceneId.MainLevel, "uid://bqhfk8hfk4s4c" }
    };

    public static string ValidateUids() {
        StringBuilder errors = new();
        int errorCount = 0;
        SceneId[] sceneIds = Enum.GetValues<SceneId>();

        foreach (SceneId sceneId in sceneIds) {
            if (!_sceneUid.TryGetValue(sceneId, out string uid)) {
                errors.AppendLine($"Scenes.{sceneId} [MISSING UID]");
                errorCount++;
                continue;
            }

            long id = ResourceUid.TextToId(uid);

            if (id == ResourceUid.InvalidId || !ResourceUid.HasId(id)) {
                errors.AppendLine($"Scenes.{sceneId} [INVALID UID: {uid}]");
                errorCount++;
                continue;
            }

            string path = ResourceUid.GetIdPath(id);

            if (ResourceLoader.Exists(path)) {
                continue;
            }

            errors.AppendLine($"Scenes.{sceneId}: Resource missing: '{path}' ({uid}).");
            errorCount++;
        }

        StringBuilder result = new();
        result.AppendLine($"Scene validation: {sceneIds.Length} checked, {errorCount} errors.");

        if (errorCount > 0) {
            result.Append(errors);
        }

        return result.ToString();
    }

    public static string ValidateResourceNames() {
        StringBuilder valid = new();
        StringBuilder missing = new();
        StringBuilder invalid = new();
        int missingCount = 0;
        int invalidCount = 0;

        foreach (SceneId sceneId in Enum.GetValues<SceneId>()) {
            if (!_sceneUid.TryGetValue(sceneId, out string uid)) {
                missing.AppendLine($"Scene.{sceneId} name [MISSING UID]");
                missingCount++;
                continue;
            }

            long id = ResourceUid.TextToId(uid);
            if (id == ResourceUid.InvalidId || !ResourceUid.HasId(id)) {
                invalid.AppendLine($"Scene.{sceneId} name [INVALID UID: {uid}]");
                invalidCount++;
                continue;
            }

            string path = ResourceUid.GetIdPath(id);
            string name = path.GetFile().GetBaseName();

            valid.AppendLine($"Scene.{sceneId} name {name}");
        }

        StringBuilder result = new();
        int errorCount = missingCount + invalidCount;

        if (errorCount > 0) {
            result.AppendLine($"Errors: {errorCount} (missing: {missingCount}, invalid: {invalidCount})");
            result.Append(missing);
            result.Append(invalid);

            if (valid.Length > 0) {
                result.AppendLine();
            }
        }

        result.Append(valid);
        return result.ToString();
    }
}

public enum SceneId {
    MainLevel
}