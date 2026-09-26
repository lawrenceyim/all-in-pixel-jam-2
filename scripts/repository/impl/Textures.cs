using System.Collections.Generic;

namespace AddOns.Repository;

public class Textures : IRepository<TextureId> {
    private static readonly Dictionary<TextureId, string> _textureUid = new() {
        { TextureId.Icon, "uid://dmt0k34pglr1a" },
        { TextureId.BlueKeyCard, "uid://3y82wxixdukp" },
        { TextureId.GreenKeyCard, "uid://b8kp25gmlh53n" },
        { TextureId.RedKeyCard, "uid://b0rqb5hdjuhk" },
    };

    public static string ValidateUids() => RepositoryValidator.Validate(_textureUid, nameof(Textures));

    public static string GetUid(TextureId id) {
        return _textureUid[id];
    }
}

public enum TextureId {
    Icon,
    BlueKeyCard,
    GreenKeyCard,
    RedKeyCard,
}