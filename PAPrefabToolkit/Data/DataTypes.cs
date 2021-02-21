using System;
using System.Numerics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PAPrefabToolkit.Data
{
    /// <summary>
    /// The Prefab class. Contains all properties of a prefab.
    /// </summary>
    public class Prefab
    {
        /// <summary>
        /// The Prefab's name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The Prefab's type.
        /// </summary>
        public PrefabType Type;

        /// <summary>
        /// The Prefab's offset.
        /// </summary>
        public float Offset;

        /// <summary>
        /// Prefab Objects list. Contains all objects in a prefab.
        /// </summary>
        public List<PrefabObject> Objects;
    }

    /// <summary>
    /// The Prefab Object class.
    /// </summary>
    public class PrefabObject
    {
        public class EditorData
        {
            public bool Locked;
            public bool Collapse;
            public int Bin;
            public int Layer;
        }

        public class Events
        {
            public struct PositionEvent
            {
                public float Time;
                public float X;
                public float Y;
            }

            public struct ScaleEvent
            {
                public float Time;
                public float X;
                public float Y;
            }

            public struct RotationEvent
            {
                public float Time;
                public float X;
            }

            public struct ColorEvent
            {
                public float Time;
                public float X;
            }

            public List<PositionEvent> PositionEvents = new List<PositionEvent>() { new PositionEvent() };
            public List<ScaleEvent> ScaleEvents = new List<ScaleEvent>() { new ScaleEvent() };
            public List<RotationEvent> RotationEvents = new List<RotationEvent>() { new RotationEvent() };
            public List<ColorEvent> ColorEvents = new List<ColorEvent>() { new ColorEvent() };
        }

        internal PrefabObject()
        {

        }

        /// <summary>
        /// The Object's Id.
        /// </summary>
        public string Id;

        /// <summary>
        /// The Object's Parent Type.
        /// </summary>
        public (bool PositionParenting, bool ScaleParenting, bool RotationParenting) ParentType = (true, false, true);

        /// <summary>
        /// The offset from parent.
        /// </summary>
        public float[] ParentOffset = new float[3];

        /// <summary>
        /// The Object's Parent's Id.
        /// </summary>
        public string ParentId;

        /// <summary>
        /// The Object's Render depth.
        /// </summary>
        public int Depth;

        /// <summary>
        /// The Object's type.
        /// </summary>
        public PrefabObjectType ObjectType;

        /// <summary>
        /// The Object's start time.
        /// </summary>
        public float StartTime;

        /// <summary>
        /// The Object's text. (Only used for text objects)
        /// </summary>
        public string Text;

        /// <summary>
        /// The Object's name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The Object's shape.
        /// </summary>
        public PrefabObjectShape Shape;

        /// <summary>
        /// The Object's auto kill type.
        /// </summary>
        public PrefabObjectAutoKillType AutoKillType;

        //TODO: Someone add the rest of the comments thanks a lot.

        public float AutoKillOffset;

        public int ShapeOption;

        public Vector2 Origin;

        public EditorData Editor = new EditorData();

        public Events ObjectEvents = new Events();
    }
}
