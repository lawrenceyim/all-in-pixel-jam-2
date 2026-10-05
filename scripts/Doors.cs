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
}