using System;

namespace PAPrefabToolkit
{
    /// <summary>
    /// Prefab Type enum. Contains all possible prefab types.
    /// </summary>
    public enum PrefabType : int
    {
        Bombs = 0,
        Bullets = 1,
        Beams = 2,
        Spinners = 3,
        Pulses = 4,
        Characters = 5,
        Misc1 = 6,
        Misc2 = 7,
        Misc3 = 8,
        Misc4 = 9
    }

    /// <summary>
    /// Prefab Object Type enum. Contains all possible object types.
    /// </summary>
    public enum PrefabObjectType : int
    {
        Normal = 0,
        Helper = 1,
        Decoration = 2,
        Empty = 3
    }

    /// <summary>
    /// Prefab Object Shape enum. Contains all possible object shapes.
    /// </summary>
    public enum PrefabObjectShape : int
    {
        Square = 0,
        Circle = 1,
        Triangle = 2,
        ArrowUp = 3,
        Text = 4,
        Hexagon = 5
    }

    /// <summary>
    /// Prefab Square Option enum. Contains all possible options for the Square shape.
    /// </summary>
    public enum PrefabSquareOption : int
    {
        Solid = 0,
        HollowThick = 1,
        HollowThin = 2
    }

    /// <summary>
    /// Prefab Circle Option enum. Contains all possible options for the Circle shape.
    /// </summary>
    public enum PrefabCircleOption : int
    {
        Solid = 0,
        HollowThick = 1,
        HalfSolid = 2,
        HalfHollow = 3,
        HollowThin = 4,
        QuarterSolid = 5,
        QuarterHollow = 6,
        HalfQuarterSolid = 7,
        HalfQuarterHollow = 8
    }

    /// <summary>
    /// Prefab Triangle Option enum. Contains all possible options for the Triangle shape.
    /// </summary>
    public enum PrefabTriangleOption : int
    {
        Solid = 0,
        Hollow = 1,
        RightAngledSolid = 2,
        RightAngledHollow = 3
    }

    /// <summary>
    /// Prefab Arrow Option enum. Contains all possible options for the Arrow shape.
    /// </summary>
    public enum PrefabArrowOption : int
    {
        Normal = 0,
        Head = 1
    }

    /// <summary>
    /// Prefab Hexagon Option enum. Contains all possible options for the Hexagon shape.
    /// </summary>
    public enum PrefabHexagonOption : int
    {
        Solid = 0,
        HollowThick = 1,
        HollowThin = 2,
        Half = 3,
        HalfHollowThick = 4,
        HalfHollowThin = 5
    }

    /// <summary>
    /// Prefab Object Auto Kill Type enum. Contains all possible object auto kill types.
    /// </summary>
    public enum PrefabObjectAutoKillType : int
    {
        [Obsolete("This auto kill type is obsolete and will be removed in future versions of Project Arrhythmia.")]
        NoAutokill = 0,

        LastKeyframe = 1,
        LastKeyframeOffset = 2,
        Fixed = 3,
        SongTime = 4
    }

    /// <summary>
    /// Prefab Object Easing enum. Contains all possible object curve types.
    /// </summary>
    public enum PrefabObjectEasing
    {
        Linear,
        Instant,
        InSine,
        OutSine,
        InOutSine,
        InElastic,
        OutElastic,
        InOutElastic,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce,
        InQuad,
        OutQuad,
        InOutQuad,
        InCirc,
        OutCirc,
        InOutCirc,
        InExpo,
        OutExpo,
        InOutExpo
    }

    /// <summary>
    /// Prefab Object Random Mode enum. Contains all possible object random modes.
    /// </summary>
    public enum PrefabObjectRandomMode : int
    {
        None = 0,
        Range = 1,
        Select = 3,
        Scale = 4
    }

    /// <summary>
    /// Prefab Build Flags enum. Used for setting certain configurations to the prefab builder.
    /// </summary>
    public enum PrefabBuildFlags : byte
    {
        None             = 0b00000000,
        SortObjects      = 0b00000001,
        SortKeyframes    = 0b00000010,
        AbsoluteRotation = 0b00000100
    }
}