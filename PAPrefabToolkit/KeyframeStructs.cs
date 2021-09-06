using System.Numerics;

namespace PAPrefabToolkit
{
    public struct PositionKeyframe
    {
        public float Time;
        public Vector2 Value;

        public PrefabObjectEasing Easing;

        public PrefabObjectRandomMode RandomMode;

        /// <summary>
        /// Ignored if random mode is none.
        /// </summary>
        public Vector2 RandomValue;

        /// <summary>
        /// Ignored if random mode is none.
        /// </summary>
        public float RandomInterval;
    }

    public struct ScaleKeyframe
    {
        public float Time;
        public Vector2 Value;

        public PrefabObjectEasing Easing;

        public PrefabObjectRandomMode RandomMode;

        /// <summary>
        /// Ignored if random mode is none.
        /// </summary>
        public Vector2 RandomValue;

        /// <summary>
        /// Ignored if random mode is none.
        /// </summary>
        public float RandomInterval;
    }

    public struct RotationKeyframe
    {
        public float Time;
        public float Value;

        public PrefabObjectEasing Easing;

        public PrefabObjectRandomMode RandomMode;

        /// <summary>
        /// Ignored if random mode is none.
        /// </summary>
        public float RandomValue;

        /// <summary>
        /// Ignored if random mode is none.
        /// </summary>
        public float RandomInterval;
    }

    public struct ColorKeyframe
    {
        public float Time;
        public int Value;

        public PrefabObjectEasing Easing;
    }
}
