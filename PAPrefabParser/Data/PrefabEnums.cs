using System;
using System.Collections.Generic;
using System.Text;

namespace PAPrefabParser.Data
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
        Hexagon = 1,
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
}
