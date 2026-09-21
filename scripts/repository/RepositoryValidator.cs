using System;
using System.Collections.Generic;
using System.Text;
using Godot;

namespace Repository;

public static class RepositoryValidator {
    public static string Validate<TId>(
        IReadOnlyDictionary<TId, string> uids,
        string repositoryName
    ) where TId : struct, Enum {
        StringBuilder errors = new();
        StringBuilder names = new();
        TId[] ids = Enum.GetValues<TId>();
        int errorCount = 0;

        foreach (TId id in ids) {
            string label = $"{repositoryName} {id}";

            if (!uids.TryGetValue(id, out string uid)) {
                AddError($"{label} [MISSING UID]");
                continue;
            }

            long resourceId = string.IsNullOrWhiteSpace(uid)
                ? ResourceUid.InvalidId
                : ResourceUid.TextToId(uid);

            if (resourceId == ResourceUid.InvalidId ||
                !ResourceUid.HasId(resourceId)) {
                AddError($"{label} [INVALID UID: {uid}]");
                continue;
            }

            string path = ResourceUid.GetIdPath(resourceId);
            if (!ResourceLoader.Exists(path)) {
                AddError($"{label} [RESOURCE MISSING: {path}]");
                continue;
            }

            names.AppendLine($"{label} name {path.GetFile().GetBaseName()}");
        }

        StringBuilder result = new();
        result.AppendLine(
            $"{repositoryName} validation: {ids.Length} checked, {errorCount} errors.");

        if (errorCount > 0) {
            result.Append(errors);
        }

        if (names.Length <= 0) {
            return result.ToString();
        }

        result.AppendLine();
        result.Append(names);

        return result.ToString();

        void AddError(string message) {
            errors.AppendLine(message);
            errorCount++;
        }
    }
}