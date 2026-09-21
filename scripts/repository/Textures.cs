using System.Collections.Generic;

namespace Repository;

public class Textures : IRepository<TextureId> {
    private static readonly Dictionary<TextureId, string> _textureUid = new() {
        { TextureId.Icon, "uid://dmt0k34pglr1a" }
    };

    public static string ValidateUids() => RepositoryValidator.Validate(_textureUid, nameof(Textures));

    public static string GetUid(TextureId id) {
        return _textureUid[id];
    }
}

public enum TextureId {
    Icon
}