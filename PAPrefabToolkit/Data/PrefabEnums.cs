namespace PAPrefabToolkit.Data
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
        Misc_1 = 6,
        Misc_2 = 7,
        Misc_3 = 8,
        Misc_4 = 9
    }

    /// <summary>
    /// Prefab Object Type enum. Contains all possible object types.
    /// </summary>
    public enum PrefabObjectType : int
    {
        Normal = 0,
        Helper = 1,
        Deco = 2,
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
        Pentagon = 5
    }

    /// <summary>
    /// Prefab Object Auto Kill Type enum. Contains all possible object auto kill types.
    /// </summary>
    public enum PrefabObjectAutoKillType : int
    {
        None = 0,
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
}
