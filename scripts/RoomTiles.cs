using System;
using System.Collections.Generic;
using AddOns.Repository;
using Godot;

public enum TileId {
    Placeholder
}

public static class RoomTiles {
    public const int InteriorWidth = 6;
    public const int InteriorHeight = 5;

    public const int TotalWidth = InteriorWidth + 2;
    public const int TotalHeight = InteriorHeight + 2;

    private enum Layout {
        None,
        Left,
        Right,
        Both,
        Start
    }

    // Four shared tile layouts.
    private static readonly Dictionary<Layout, Dictionary<Vector2, TileId>> _layouts = new() {
        [Layout.None] = CreateLayout(false, false),
        [Layout.Left] = CreateLayout(true, false),
        [Layout.Right] = CreateLayout(false, true),
        [Layout.Both] = CreateLayout(true, true),
        [Layout.Start] = CreateLayout(true, true, true)
    };

    // Assumes your existing scene enum uses A, B, C, etc.
    private static readonly Dictionary<SceneId, Layout> _roomLayouts = new() {
        [SceneId.A] = Layout.None,
        [SceneId.B] = Layout.Start,
        [SceneId.C] = Layout.Left,
        [SceneId.D] = Layout.Right,
        [SceneId.E] = Layout.Both,
        [SceneId.F] = Layout.Right,
        [SceneId.G] = Layout.Both,
        [SceneId.H] = Layout.Both,
        [SceneId.I] = Layout.None,
        [SceneId.J] = Layout.Both,
        [SceneId.K] = Layout.Both,
        [SceneId.L] = Layout.Left,
        [SceneId.M] = Layout.Left,
        [SceneId.N] = Layout.Left,
        [SceneId.O] = Layout.None
    };

    public static Dictionary<Vector2, TileId> GetTiles(SceneId sceneId) {
        if (!_roomLayouts.TryGetValue(sceneId, out Layout layout)) {
            throw new ArgumentOutOfRangeException(
                nameof(sceneId), sceneId, "No tile layout for this room.");
        }

        // Return a copy so edits only affect this room instance.
        return new Dictionary<Vector2, TileId>(_layouts[layout]);
    }

    private static Dictionary<Vector2, TileId> CreateLayout(
        bool platformLeft,
        bool platformRight,
        bool openCeiling = false
    ) {
        Dictionary<Vector2, TileId> tiles = new();

        // Ceiling: Y = 0
        // Floor:   Y = 6
        for (int x = 0; x < TotalWidth; x++) {
            tiles[new Vector2(x, 0)] = TileId.Placeholder;
            tiles[new Vector2(x, TotalHeight - 1)] = TileId.Placeholder;
        }

        // Left wall:  X = 0
        // Right wall: X = 7
        for (int y = 1; y < TotalHeight - 1; y++) {
            tiles[new Vector2(0, y)] = TileId.Placeholder;
            tiles[new Vector2(TotalWidth - 1, y)] = TileId.Placeholder;
        }

        // Platforms are one tile thick.
        // Two empty rows above and two below.
        const int platformY = 3;

        if (platformLeft) {
            tiles[new Vector2(1, platformY)] = TileId.Placeholder;
            tiles[new Vector2(2, platformY)] = TileId.Placeholder;
        }

        if (platformRight) {
            tiles[new Vector2(5, platformY)] = TileId.Placeholder;
            tiles[new Vector2(6, platformY)] = TileId.Placeholder;
        }

        if (openCeiling) {
            tiles.Remove(new Vector2(3, 0));
            tiles.Remove(new Vector2(4, 0));
            tiles.Add(new Vector2(2, -1), TileId.Placeholder);
            tiles.Add(new Vector2(5, -1), TileId.Placeholder);
            tiles.Add(new Vector2(2, -2), TileId.Placeholder);
            tiles.Add(new Vector2(5, -2), TileId.Placeholder);
            tiles.Add(new Vector2(2, -3), TileId.Placeholder);
            tiles.Add(new Vector2(5, -3), TileId.Placeholder);
            tiles.Add(new Vector2(2, -4), TileId.Placeholder);
            tiles.Add(new Vector2(5, -4), TileId.Placeholder);
        }

        return tiles;
    }
}

public static class TileMapFactory {
    private const int PlaceholderSourceId = 0;
    private static readonly Vector2I _placeholderAtlasCoords = new(0, 0);

    public static TileMapLayer CreateTileMap(
        Dictionary<Vector2, TileId> tiles
    ) {
        TileSet tileSet = GD.Load<TileSet>("uid://bb5f3v0phem3p");

        ArgumentNullException.ThrowIfNull(tiles);
        ArgumentNullException.ThrowIfNull(tileSet);

        TileMapLayer tileMap = new() {
            Name = "RoomTiles",
            TileSet = tileSet
        };

        foreach ((Vector2 position, TileId value) in tiles) {
            Vector2I cell = new((int)position.X, (int)position.Y);
            switch (value) {
                case TileId.Placeholder:
                    tileMap.SetCell(
                        cell,
                        PlaceholderSourceId,
                        _placeholderAtlasCoords
                    );
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(tiles), value, "Unknown tile ID.");
            }
        }

        return tileMap;
    }
}