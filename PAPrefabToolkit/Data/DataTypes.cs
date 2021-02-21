using System.Collections.Generic;
using System.Numerics;

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
        /// <summary>
        /// The EditorData clas. Contains object's editor data.
        /// </summary>
        public class EditorData
        {
            internal EditorData() { }

            /// <summary>
            /// The Editor locked state.
            /// </summary>
            public bool Locked;

            /// <summary>
            /// The Editor collapsed state.
            /// </summary>
            public bool Collapse;

            /// <summary>
            /// The Editor bin.
            /// </summary>
            public int Bin;

            /// <summary>
            /// The Editor layer.
            /// </summary>
            public int Layer;
        }

        /// <summary>
        /// The Events class. Contains object events.
        /// </summary>
        public class Events
        {
            internal Events() { }

            /// <summary>
            /// The Object's position event struct.
            /// </summary>
            public struct PositionEvent
            {
                public float Time;
                public float X;
                public float Y;
            }

            /// <summary>
            /// The Object's scale event struct.
            /// </summary>
            public struct ScaleEvent
            {
                public float Time;
                public float X;
                public float Y;
            }

            /// <summary>
            /// The Object's rotation event struct.
            /// </summary>
            public struct RotationEvent
            {
                public float Time;
                public float X;
            }

            /// <summary>
            /// The Object's color event struct.
            /// </summary>
            public struct ColorEvent
            {
                public float Time;
                public float X;
            }

            /// <summary>
            /// The Object's position events list.
            /// </summary>
            public List<PositionEvent> PositionEvents = new List<PositionEvent>() { new PositionEvent() };

            /// <summary>
            /// The Object's scale events list.
            /// </summary>
            public List<ScaleEvent> ScaleEvents = new List<ScaleEvent>() { new ScaleEvent() };

            /// <summary>
            /// The Object's rotation events list.
            /// </summary>
            public List<RotationEvent> RotationEvents = new List<RotationEvent>() { new RotationEvent() };

            /// <summary>
            /// The Object's color events list.
            /// </summary>
            public List<ColorEvent> ColorEvents = new List<ColorEvent>() { new ColorEvent() };
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
        public int Depth = 15;

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

        /// <summary>
        /// The Object's auto kill offset.
        /// </summary>
        public float AutoKillOffset;

        /// <summary>
        /// The Object's shape option.
        /// </summary>
        public int ShapeOption;

        /// <summary>
        /// The Object's origin.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// The Object's editor data.
        /// </summary>
        public EditorData Editor = new EditorData();

        /// <summary>
        /// The Object's events.
        /// </summary>
        public Events ObjectEvents = new Events();
    }
}
