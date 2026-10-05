using System.Collections.Generic;
using AddOns.Repository;
using Godot;

public class Doors {
    public enum DoorId {
        AToB,
        AToD,

        BToA,
        BToC,
        BToE,

        CToB,
        CToF,

        DToA,
        DToE,
        DToG,

        EToB,
        EToD,
        EToF,
        EToH,

        FToC,
        FToE,

        GToD,
        GToH,
        GToJ,

        HToE,
        HToG,
        HToI,
        HToK,

        IToH,
        IToL,

        JToG,
        JToK,
        JToM,

        KToH,
        KToJ,
        KToL,
        KToN,

        LToI,
        LToK,
        LToO,

        MToJ,
        MToN,

        NToK,
        NToM,
        NToO,

        OToL,
        OToN,

        Goal
    }

    private static readonly Dictionary<DoorId, List<KeyCard.Color>> _doorColors = new() {
        [DoorId.AToB] = [KeyCard.Color.Red],
        [DoorId.AToD] = [KeyCard.Color.Red],

        [DoorId.BToA] = [KeyCard.Color.Red],
        [DoorId.BToC] = [KeyCard.Color.Green],
        [DoorId.BToE] = [KeyCard.Color.Blue],

        [DoorId.CToB] = [KeyCard.Color.Green],
        [DoorId.CToF] = [KeyCard.Color.Green],

        [DoorId.DToA] = [KeyCard.Color.Red],
        [DoorId.DToE] = [KeyCard.Color.Red],
        [DoorId.DToG] = [KeyCard.Color.Blue],

        [DoorId.EToB] = [KeyCard.Color.Blue],
        [DoorId.EToD] = [KeyCard.Color.Red],
        [DoorId.EToF] = [KeyCard.Color.Red],
        [DoorId.EToH] = [KeyCard.Color.Red],

        [DoorId.FToC] = [KeyCard.Color.Green],
        [DoorId.FToE] = [KeyCard.Color.Red],

        [DoorId.GToD] = [KeyCard.Color.Blue],
        [DoorId.GToH] = [KeyCard.Color.Blue],
        [DoorId.GToJ] = [KeyCard.Color.Blue],

        [DoorId.HToE] = [KeyCard.Color.Red],
        [DoorId.HToG] = [KeyCard.Color.Blue],
        [DoorId.HToI] = [KeyCard.Color.Red],
        [DoorId.HToK] = [KeyCard.Color.Blue],

        [DoorId.IToH] = [KeyCard.Color.Red],
        [DoorId.IToL] = [KeyCard.Color.Red],

        [DoorId.JToG] = [KeyCard.Color.Blue],
        [DoorId.JToK] = [KeyCard.Color.Red],
        [DoorId.JToM] = [KeyCard.Color.Red],

        [DoorId.KToH] = [KeyCard.Color.Blue],
        [DoorId.KToJ] = [KeyCard.Color.Red],
        [DoorId.KToL] = [KeyCard.Color.Blue],
        [DoorId.KToN] = [KeyCard.Color.Blue],

        [DoorId.LToI] = [KeyCard.Color.Red],
        [DoorId.LToK] = [KeyCard.Color.Blue],
        [DoorId.LToO] = [KeyCard.Color.Blue],

        [DoorId.MToJ] = [KeyCard.Color.Red],
        [DoorId.MToN] = [KeyCard.Color.Green],

        [DoorId.NToK] = [KeyCard.Color.Blue],
        [DoorId.NToM] = [KeyCard.Color.Green],
        [DoorId.NToO] = [KeyCard.Color.Green],

        [DoorId.OToL] = [KeyCard.Color.Blue],
        [DoorId.OToN] = [KeyCard.Color.Green],

        [DoorId.Goal] = [
            KeyCard.Color.Red,
            KeyCard.Color.Blue,
            KeyCard.Color.Green
        ]
    };

    private static readonly Dictionary<SceneId, List<DoorId>> _roomDoors = new() {
        [SceneId.A] = [DoorId.AToB, DoorId.AToD],
        [SceneId.B] = [DoorId.BToA, DoorId.BToE, DoorId.Goal, DoorId.BToC],
        [SceneId.C] = [DoorId.CToB, DoorId.CToF],
        [SceneId.D] = [DoorId.DToE, DoorId.DToA, DoorId.DToG],
        [SceneId.E] = [DoorId.EToB, DoorId.EToF, DoorId.EToD, DoorId.EToH],
        [SceneId.F] = [DoorId.FToE, DoorId.FToC],
        [SceneId.G] = [DoorId.GToD, DoorId.GToH, DoorId.GToJ],
        [SceneId.H] = [DoorId.HToE, DoorId.HToI, DoorId.HToG, DoorId.HToK],
        [SceneId.I] = [DoorId.IToH, DoorId.IToL],
        [SceneId.J] = [DoorId.JToG, DoorId.JToK, DoorId.JToM],
        [SceneId.K] = [DoorId.KToH, DoorId.KToN, DoorId.KToJ, DoorId.KToL],
        [SceneId.L] = [DoorId.LToI, DoorId.LToK, DoorId.LToO],
        [SceneId.M] = [DoorId.MToJ, DoorId.MToN],
        [SceneId.N] = [DoorId.NToK, DoorId.NToM, DoorId.NToO],
        [SceneId.O] = [DoorId.OToN, DoorId.OToL]
    };

    public enum DoorLocation {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public static DoorLocation GetDoorLocation(DoorId doorId) {
        return doorId switch {
            DoorId.AToB => DoorLocation.BottomLeft,
            DoorId.AToD => DoorLocation.BottomRight,

            DoorId.BToA => DoorLocation.TopLeft,
            DoorId.BToC => DoorLocation.BottomRight,
            DoorId.BToE => DoorLocation.TopRight,

            DoorId.CToB => DoorLocation.TopLeft,
            DoorId.CToF => DoorLocation.BottomRight,

            DoorId.DToA => DoorLocation.BottomLeft,
            DoorId.DToE => DoorLocation.TopRight,
            DoorId.DToG => DoorLocation.BottomRight,

            DoorId.EToB => DoorLocation.TopLeft,
            DoorId.EToD => DoorLocation.BottomLeft,
            DoorId.EToF => DoorLocation.TopRight,
            DoorId.EToH => DoorLocation.BottomRight,

            DoorId.FToC => DoorLocation.BottomRight,
            DoorId.FToE => DoorLocation.BottomLeft,

            DoorId.GToD => DoorLocation.TopLeft,
            DoorId.GToH => DoorLocation.TopRight,
            DoorId.GToJ => DoorLocation.BottomRight,

            DoorId.HToE => DoorLocation.TopLeft,
            DoorId.HToG => DoorLocation.BottomLeft,
            DoorId.HToI => DoorLocation.TopRight,
            DoorId.HToK => DoorLocation.BottomRight,

            DoorId.IToH => DoorLocation.BottomLeft,
            DoorId.IToL => DoorLocation.BottomRight,

            DoorId.JToG => DoorLocation.TopLeft,
            DoorId.JToK => DoorLocation.TopRight,
            DoorId.JToM => DoorLocation.BottomLeft,

            DoorId.KToH => DoorLocation.TopLeft,
            DoorId.KToJ => DoorLocation.BottomLeft,
            DoorId.KToL => DoorLocation.BottomRight,
            DoorId.KToN => DoorLocation.TopRight,

            DoorId.LToI => DoorLocation.TopLeft,
            DoorId.LToK => DoorLocation.BottomLeft,
            DoorId.LToO => DoorLocation.BottomRight,

            DoorId.MToJ => DoorLocation.BottomLeft,
            DoorId.MToN => DoorLocation.BottomRight,

            DoorId.NToK => DoorLocation.TopLeft,
            DoorId.NToM => DoorLocation.BottomLeft,
            DoorId.NToO => DoorLocation.BottomRight,

            DoorId.OToL => DoorLocation.BottomRight,
            DoorId.OToN => DoorLocation.BottomLeft,

            DoorId.Goal => DoorLocation.BottomLeft,

            _ => throw new System.ArgumentOutOfRangeException(
                nameof(doorId), doorId, "Unknown door ID.")
        };
    }

    public static Vector2 DoorPosition(DoorLocation location) {
        return location switch {
            DoorLocation.TopLeft => new Vector2(56, 68),
            DoorLocation.TopRight => new Vector2(201, 68),
            DoorLocation.BottomLeft => new Vector2(56, 164),
            DoorLocation.BottomRight => new Vector2(201, 164),
        };
    }


    public static void SpawnDoors(Node parent, SceneId sceneId) {
        PackedScene doorScene = GD.Load<PackedScene>(
            GameObjects.GetUid(GameObjectId.Door)
        );

        foreach (DoorId id in _roomDoors[sceneId]) {
            Door door = doorScene.Instantiate<Door>();
            door.Init(id, _doorColors[id]);
            door.Position = DoorPosition(GetDoorLocation(id));

            parent.AddChild(door);
        }
    }

    public static DoorId GetDestinationDoorId(DoorId id) {
        return id switch {
            DoorId.AToB => DoorId.BToA,
            DoorId.AToD => DoorId.DToA,

            DoorId.BToA => DoorId.AToB,
            DoorId.BToC => DoorId.CToB,
            DoorId.BToE => DoorId.EToB,

            DoorId.CToB => DoorId.BToC,
            DoorId.CToF => DoorId.FToC,

            DoorId.DToA => DoorId.AToD,
            DoorId.DToE => DoorId.EToD,
            DoorId.DToG => DoorId.GToD,

            DoorId.EToB => DoorId.BToE,
            DoorId.EToD => DoorId.DToE,
            DoorId.EToF => DoorId.FToE,
            DoorId.EToH => DoorId.HToE,

            DoorId.FToC => DoorId.CToF,
            DoorId.FToE => DoorId.EToF,

            DoorId.GToD => DoorId.DToG,
            DoorId.GToH => DoorId.HToG,
            DoorId.GToJ => DoorId.JToG,

            DoorId.HToE => DoorId.EToH,
            DoorId.HToG => DoorId.GToH,
            DoorId.HToI => DoorId.IToH,
            DoorId.HToK => DoorId.KToH,

            DoorId.IToH => DoorId.HToI,
            DoorId.IToL => DoorId.LToI,

            DoorId.JToG => DoorId.GToJ,
            DoorId.JToK => DoorId.KToJ,
            DoorId.JToM => DoorId.MToJ,

            DoorId.KToH => DoorId.HToK,
            DoorId.KToJ => DoorId.JToK,
            DoorId.KToL => DoorId.LToK,
            DoorId.KToN => DoorId.NToK,

            DoorId.LToI => DoorId.IToL,
            DoorId.LToK => DoorId.KToL,
            DoorId.LToO => DoorId.OToL,

            DoorId.MToJ => DoorId.JToM,
            DoorId.MToN => DoorId.NToM,

            DoorId.NToK => DoorId.KToN,
            DoorId.NToM => DoorId.MToN,
            DoorId.NToO => DoorId.OToN,

            DoorId.OToL => DoorId.LToO,
            DoorId.OToN => DoorId.NToO,

            DoorId.Goal => throw new System.InvalidOperationException(
                "The goal door has no destination door."),

            _ => throw new System.ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }

    public static SceneId GetDestinationScene(DoorId id) {
        return id switch {
            DoorId.BToA or DoorId.DToA => SceneId.A,

            DoorId.AToB or DoorId.CToB or DoorId.EToB => SceneId.B,

            DoorId.BToC or DoorId.FToC => SceneId.C,

            DoorId.AToD or DoorId.EToD or DoorId.GToD => SceneId.D,

            DoorId.BToE or DoorId.DToE or DoorId.FToE or DoorId.HToE
                => SceneId.E,

            DoorId.CToF or DoorId.EToF => SceneId.F,

            DoorId.DToG or DoorId.HToG or DoorId.JToG => SceneId.G,

            DoorId.EToH or DoorId.GToH or DoorId.IToH or DoorId.KToH
                => SceneId.H,

            DoorId.HToI or DoorId.LToI => SceneId.I,

            DoorId.GToJ or DoorId.KToJ or DoorId.MToJ => SceneId.J,

            DoorId.HToK or DoorId.JToK or DoorId.LToK or DoorId.NToK
                => SceneId.K,

            DoorId.IToL or DoorId.KToL or DoorId.OToL => SceneId.L,

            DoorId.JToM or DoorId.NToM => SceneId.M,

            DoorId.KToN or DoorId.MToN or DoorId.OToN => SceneId.N,

            DoorId.LToO or DoorId.NToO => SceneId.O,

            DoorId.Goal => throw new System.InvalidOperationException(
                "The goal door has no destination scene."),

            _ => throw new System.ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
}