using System;
using System.Collections;
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

        internal Prefab() { }

        /// <summary>
        /// The constructor of the Prefab type.
        /// </summary>
        /// <param name="name">The name of the prefab.</param>
        /// <param name="prefabType">The type of the prefab.</param>
        public Prefab(string name, PrefabType prefabType = PrefabType.Bombs)
        {
            Name = name;
            Type = prefabType;
        }

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
            throw new Exception($"Object {prefabObject.Name} shares the same ID with another object!\nConsider using Prefab.GenId() method to prevent collisions.");
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
            while (ids.Contains(str));

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
        /// The ObjectEvents class. Contains object events.
        /// </summary>
        public class ObjectEvents
        {
            internal ObjectEvents() { }

            #region KeyframesDef
            public interface IKeyframe
            {
                public float Time { get; set; }
            }

            //position struct
            public struct PositionKeyframe : IKeyframe
            {
                public float Time { get; set; }
                public Vector2 Value;

                public PrefabObjectEasing Easing;

                public PrefabObjectRandomMode RandomMode;
                public Vector2 RandomValue;
                public float RandomInterval;
            }

            //scale struct
            public struct ScaleKeyframe : IKeyframe
            {
                public float Time { get; set; }
                public Vector2 Value;

                public PrefabObjectEasing Easing;

                public PrefabObjectRandomMode RandomMode;
                public Vector2 RandomValue;
                public float RandomInterval;
            }

            //rotation struct
            public struct RotationKeyframe : IKeyframe
            {
                public float Time { get; set; }
                public float Value;

                public PrefabObjectEasing Easing;

                public PrefabObjectRandomMode RandomMode;
                public float RandomValue;
                public float RandomInterval;
            }

            //color struct
            public struct ColorKeyframe : IKeyframe
            {
                public float Time { get; set; }
                public int Value;

                public PrefabObjectEasing Easing;
            }
            #endregion

            public class KeyframeList<T> where T : IKeyframe
            {
                private List<T> keyframes = new List<T>();

                /// <summary>
                /// Get number of keyframes.
                /// </summary>
                public int Count 
                    => keyframes.Count;

                public T this[int index]
                    => keyframes[index];

                internal KeyframeList(T defaultValue)
                {
                    keyframes.Add(defaultValue);
                }

                internal KeyframeList(List<T> newList)
                {
                    keyframes = newList;
                }

                internal List<T> GetInternalList()
                    => keyframes;

                /// <summary>
                /// Add a keyframe to the sequence.
                /// </summary>
                /// <param name="keyframe">A keyframe.</param>
                public void Add(T keyframe)
                {
                    if (keyframe.Time == 0.0f)
                    {
                        keyframes[0] = keyframe;
                        return;
                    }

                    if (keyframe.Time > keyframes[keyframes.Count - 1].Time)
                    {
                        keyframes.Add(keyframe);
                        return;
                    }

                    keyframes.Insert(FindIndexForSortedInsert(keyframe), keyframe);
                }

                //algorithms
                private int FindIndexForSortedInsert(T item)
                {
                    if (keyframes.Count == 0)
                    {
                        return 0;
                    }

                    int lowerIndex = 0;
                    int upperIndex = keyframes.Count - 1;
                    int comparisonResult;
                    while (lowerIndex < upperIndex)
                    {
                        int middleIndex = (lowerIndex + upperIndex) / 2;
                        T middle = keyframes[middleIndex];
                        comparisonResult = middle.Time.CompareTo(item.Time);
                        if (comparisonResult == 0)
                        {
                            return middleIndex;
                        }
                        else if (comparisonResult > 0) // middle > item
                        {
                            upperIndex = middleIndex - 1;
                        }
                        else // middle < item
                        {
                            lowerIndex = middleIndex + 1;
                        }
                    }

                    comparisonResult = keyframes[lowerIndex].Time.CompareTo(item.Time);
                    if (comparisonResult < 0)
                    {
                        return lowerIndex + 1;
                    }
                    else
                    {
                        return lowerIndex;
                    }
                }
            }

            public KeyframeList<PositionKeyframe> PositionKeyframes { get; internal set; } = new KeyframeList<PositionKeyframe>(new PositionKeyframe { Value = Vector2.Zero });
            public KeyframeList<ScaleKeyframe> ScaleKeyframes { get; internal set; } = new KeyframeList<ScaleKeyframe>(new ScaleKeyframe { Value = Vector2.One });
            public KeyframeList<RotationKeyframe> RotationKeyframes { get; internal set; } = new KeyframeList<RotationKeyframe>(new RotationKeyframe { Value = 0.0f });
            public KeyframeList<ColorKeyframe> ColorKeyframes { get; internal set; } = new KeyframeList<ColorKeyframe>(new ColorKeyframe { Value = 0 });
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
        public PrefabObjectAutoKillType AutoKillType = PrefabObjectAutoKillType.Fixed;

        /// <summary>
        /// The Object's auto kill offset.
        /// </summary>
        public float AutoKillOffset = 1.0f;

        /// <summary>
        /// The Object's shape option.
        /// </summary>
        public int ShapeOption;

        /// <summary>
        /// The Object's origin.
        /// </summary>
        public (PrefabObjectOriginX X, PrefabObjectOriginY Y) Origin = (PrefabObjectOriginX.Center, PrefabObjectOriginY.Center);

        /// <summary>
        /// The Object's editor data.
        /// </summary>
        public EditorData Editor { get; internal set; } = new EditorData();

        /// <summary>
        /// The Object's events.
        /// </summary>
        public ObjectEvents Events { get; internal set; } = new ObjectEvents();
    }
}
