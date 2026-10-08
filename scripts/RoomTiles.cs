using System;
using System.Collections.Generic;
using AddOns.Repository;
using Godot;

public enum TileId {
    Placeholder,
    Tile3,
    Tile4
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

    private static readonly Dictionary<Layout, Dictionary<Vector2, TileId>> _layouts = new() {
        [Layout.None] = CreateLayout(false, false),
        [Layout.Left] = CreateLayout(true, false),
        [Layout.Right] = CreateLayout(false, true),
        [Layout.Both] = CreateLayout(true, true),
        [Layout.Start] = CreateLayout(true, true, true)
    };

    private static readonly Dictionary<SceneId, (Layout Layout, TileId Tile)> _roomLayouts = new() {
        [SceneId.A] = (Layout.None, TileId.Tile3),
        [SceneId.B] = (Layout.Start, TileId.Tile4),
        [SceneId.C] = (Layout.Left, TileId.Tile3),
        [SceneId.D] = (Layout.Right, TileId.Tile4),
        [SceneId.E] = (Layout.Both, TileId.Tile3),
        [SceneId.F] = (Layout.Right, TileId.Tile4),
        [SceneId.G] = (Layout.Both, TileId.Tile3),
        [SceneId.H] = (Layout.Both, TileId.Tile4),
        [SceneId.I] = (Layout.None, TileId.Tile3),
        [SceneId.J] = (Layout.Both, TileId.Tile4),
        [SceneId.K] = (Layout.Both, TileId.Tile3),
        [SceneId.L] = (Layout.Left, TileId.Tile4),
        [SceneId.M] = (Layout.Left, TileId.Tile3),
        [SceneId.N] = (Layout.Left, TileId.Tile4),
        [SceneId.O] = (Layout.None, TileId.Tile3)
    };

    public static Dictionary<Vector2, TileId> GetTiles(SceneId sceneId) {
        if (!_roomLayouts.TryGetValue(sceneId, out (Layout Layout, TileId Tile) room)) {
            throw new ArgumentOutOfRangeException(nameof(sceneId), sceneId, "No tile layout for this room.");
        }

        Dictionary<Vector2, TileId> tiles = new();
        foreach (Vector2 position in _layouts[room.Layout].Keys) {
            tiles[position] = room.Tile;
        }

        return tiles;
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

        // Create chimney to prevent player from jumping out of bounds when spawned out of view
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
    public static TileMapLayer CreateTileMap(
        Dictionary<Vector2, TileId> tiles
    ) {
        TileSet tileSet = GD.Load<TileSet>("uid://bb5f3v0phem3p");
        TileMapLayer tileMap = new() {
            Name = "RoomTiles",
            TileSet = tileSet
        };

        foreach ((Vector2 position, TileId tileId) in tiles) {
            Vector2I cell = new((int)position.X, (int)position.Y);
            int sourceId = tileId switch {
                TileId.Tile3 => 3,
                TileId.Tile4 => 4,
            };

            Vector2I atlasCoords = GetAtlasCoords(cell, tiles);
            tileMap.SetCell(cell, sourceId, atlasCoords);
        }

        return tileMap;
    }

    private static Vector2I GetAtlasCoords(
        Vector2I cell,
        Dictionary<Vector2, TileId> tiles
    ) {
        const int leftWallX = 0;
        const int rightWallX = RoomTiles.TotalWidth - 1;
        const int ceilingY = 0;
        const int floorY = RoomTiles.TotalHeight - 1;

        // Entrance shaft above the starting room.
        if (cell.Y < ceilingY) {
            bool isLeftSide = cell.X < RoomTiles.TotalWidth / 2;
            return new Vector2I(isLeftSide ? 0 : 2, 1);
        }

        // Ceiling, including its two outer corners.
        if (cell.Y == ceilingY) {
            int atlasX = cell.X == leftWallX ? 0
                : cell.X == rightWallX ? 2
                : 1;

            return new Vector2I(atlasX, 0);
        }

        // Floor, including its two outer corners.
        if (cell.Y == floorY) {
            int atlasX = cell.X == leftWallX ? 0
                : cell.X == rightWallX ? 2
                : 1;

            return new Vector2I(atlasX, 2);
        }

        if (cell.X == leftWallX) {
            return new Vector2I(0, 1);
        }

        if (cell.X == rightWallX) {
            return new Vector2I(2, 1);
        }

        // Interior platforms use the floor row.
        // Use corner pieces for their exposed ends.
        bool hasLeft = tiles.ContainsKey(
            new Vector2(cell.X - 1, cell.Y));

        bool hasRight = tiles.ContainsKey(
            new Vector2(cell.X + 1, cell.Y));

        int platformAtlasX = !hasLeft ? 0
            : !hasRight ? 2
            : 1;

        return new Vector2I(platformAtlasX, 2);
    }
}