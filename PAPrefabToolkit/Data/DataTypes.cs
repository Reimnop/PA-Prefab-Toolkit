using System;
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
        internal List<PrefabObject> Objects = new List<PrefabObject>();

        //hash set to keep track of ids, to prevent collision
        private readonly HashSet<string> ids = new HashSet<string>();

        /// <summary>
        /// Add an Object to a Prefab.
        /// </summary>
        /// <param name="prefabObject">An instance of PrefabObject.</param>
        public void AddObject(PrefabObject prefabObject)
        {
            //basically idiot check
            if (!ids.Contains(prefabObject.Id))
            {
                ids.Add(prefabObject.Id);
                Objects.Add(prefabObject);
                return;
            }
            throw new System.Exception($"Object {prefabObject.Name} shares the same ID with another object!\nConsider using Prefab.GenId() method to prevent collisions.");
        }

        /// <summary>
        /// Remove an Object from a Prefab.
        /// </summary>
        /// <param name="prefabObject">An instance of PrefabObject that is previously added into this Prefab.</param>
        public void RemoveObject(PrefabObject prefabObject)
        {
            //basically idiot check
            if (Objects.Contains(prefabObject))
            {
                if (ids.Contains(prefabObject.Id))
                    ids.Remove(prefabObject.Id);

                Objects.Remove(prefabObject);
                return;
            }
            throw new System.Exception($"Object {prefabObject.Name} is not a member of this Prefab!");
        }

        /// <summary>
        /// Generates a random ID for an object. Ensures no collision can happen.
        /// </summary>
        /// <returns>A random ID.</returns>
        public string GenId()
        {
            Random rng = new Random();

            string str;
            do
                str = RandomString(rng, 16);
            while (!ids.Contains(str));

            return str;
        }

        //method to generate random string
        private string RandomString(Random random, int length)
        {
            const string rndChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~!@#$%^&*_+{}|:<>?,./;'()[]";

            string str = string.Empty;

            for (int i = 0; i < length; i++)
                str += rndChars[random.Next(0, rndChars.Length)];

            return str;
        }
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
                public PrefabObjectEasing CurveType;
            }

            /// <summary>
            /// The Object's scale event struct.
            /// </summary>
            public struct ScaleEvent
            {
                public float Time;
                public float X;
                public float Y;
                public PrefabObjectEasing CurveType;
            }

            /// <summary>
            /// The Object's rotation event struct.
            /// </summary>
            public struct RotationEvent
            {
                public float Time;
                public float X;
                public PrefabObjectEasing CurveType;
            }

            /// <summary>
            /// The Object's color event struct.
            /// </summary>
            public struct ColorEvent
            {
                public float Time;
                public float X;
                public PrefabObjectEasing CurveType;
            }

            /// <summary>
            /// The Object's position events list.
            /// </summary>
            public List<PositionEvent> PositionEvents = new List<PositionEvent>() { new PositionEvent() };

            /// <summary>
            /// The Object's scale events list.
            /// </summary>
            public List<ScaleEvent> ScaleEvents = new List<ScaleEvent>() { new ScaleEvent() { X = 1.0f, Y = 1.0f } };

            /// <summary>
            /// The Object's rotation events list.
            /// </summary>
            public List<RotationEvent> RotationEvents = new List<RotationEvent>() { new RotationEvent() };

            /// <summary>
            /// The Object's color events list.
            /// </summary>
            public List<ColorEvent> ColorEvents = new List<ColorEvent>() { new ColorEvent() };
        }

        internal PrefabObject() { }

        /// <summary>
        /// The constructor of the PrefabObject type.
        /// </summary>
        /// <param name="prefab">The prefab this object is being added to.</param>
        /// <param name="name">Name of this object.</param>
        /// <param name="id">ID of this object. (null: random ID)</param>
        /// <param name="parent">(Optional) Parent of this object.</param>
        public PrefabObject(Prefab prefab, string name, string id = null, PrefabObject parent = null)
        {
            if (name != null)
                Name = name;
            else
                throw new NullReferenceException("**name** was null.");

            if (id != null)
                Id = id;
            else
                Id = prefab.GenId();

            ParentId = parent != null ? parent.Id : string.Empty;

            prefab.AddObject(this);
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
        public (float PositionOffset, float ScaleOffset, float RotationOffset) ParentOffset = (0.0f, 0.0f, 0.0f);

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
