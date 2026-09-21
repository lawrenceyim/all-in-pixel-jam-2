using System;
using System.Collections.Generic;
using System.Text;
using Godot;

namespace Repository;

public class Textures : IRepository {
    private static readonly Dictionary<TextureId, string> _textureUid = new() {
        { TextureId.Icon, "uid://dmt0k34pglr1a" }
    };

    public static string ValidateUids() => RepositoryValidator.Validate(_textureUid, nameof(Textures));
}

public enum TextureId {
    Icon
}