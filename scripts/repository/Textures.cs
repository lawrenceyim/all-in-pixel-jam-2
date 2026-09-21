using System.Collections.Generic;
using Godot;

namespace Repository;

public static class Textures {
    private static readonly Dictionary<TextureId, string> _textureUid = new() {
        { TextureId.Icon, "uid://dmt0k34pglr1a" }
    };

    public static bool Validate() {
        int errors = 0;

        foreach ((TextureId textureId, string uid) in _textureUid) {
            long id = ResourceUid.TextToId(uid);

            if (id == ResourceUid.InvalidId || !ResourceUid.HasId(id)) {
                GD.PushError($"Textures.{textureId}: Invalid or unknown UID '{uid}'.");
                errors++;
                continue;
            }

            string path = ResourceUid.GetIdPath(id);

            if (ResourceLoader.Exists(path)) {
                continue;
            }

            GD.PushError($"Textures.{textureId}: Resource missing: '{path}' ({uid}).");
            errors++;
        }

        GD.Print($"Texture validation: {_textureUid.Count} checked, {errors} errors.");
        return errors == 0;
    }
}

public enum TextureId {
    Icon
}