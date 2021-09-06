using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace PAPrefabToolkit
{
    /// <summary>
    /// The prefab object. This will end up as an actual object in the Project Arrhythmia Editor.
    /// </summary>
    public class PrefabObject
    {
        /// <summary>
        /// The object's name.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// The object's ID. This value is chosen randomly. This field is read-only.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// The object's position parent type. If this value is true, the object's position is not affected by the parent.
        /// </summary>
        public bool PositionParenting = true;

        /// <summary>
        /// The object's scale parent type. If this value is true, the object's scale is not affected by the parent.
        /// </summary>
        public bool ScaleParenting = true;

        /// <summary>
        /// The object's rotation parent type. If this value is true, the object's rotation is not affected by the parent.
        /// </summary>
        public bool RotationParenting = true;

        /// <summary>
        /// The object's position parent offset. The object's position lags behind the parent the amount of time equals to the value.
        /// </summary>
        public float PositionParentOffset = 0.0f;

        /// <summary>
        /// The object's scale parent offset. The object's scale lags behind the parent the amount of time equals to the value.
        /// </summary>
        public float ScaleParentOffset = 0.0f;

        /// <summary>
        /// The object's position rotation offset. The object's rotation lags behind the parent the amount of time equals to the value.
        /// </summary>
        public float RotationParentOffset = 0.0f;

        public int RenderDepth = 15;

        public PrefabObjectType ObjectType = PrefabObjectType.Normal;
        public PrefabObjectShape Shape = PrefabObjectShape.Square;

        /// <summary>
        /// The object's shape option. It is recommended to use the prefab option enum for this value.
        /// </summary>
        public int ShapeOption = (int)PrefabSquareOption.Solid;

        /// <summary>
        /// The object's text. This value is ignored if object shape is not text.
        /// </summary>
        public string Text = string.Empty;

        /// <summary>
        /// The object's start time. Determines when the object is spawned.
        /// </summary>
        public float StartTime = 0.0f;

        /// <summary>
        /// The object's auto kill type. Determines when the object is killed.
        /// </summary>
        public PrefabObjectAutoKillType AutoKillType = PrefabObjectAutoKillType.LastKeyframe;

        /// <summary>
        /// The object's auto kill offset. Determines when the object is killed. Depends on auto kill type.
        /// </summary>
        public float AutoKillOffset = 0.0f;

        /// <summary>
        /// The object's origin of transformation.
        /// </summary>
        public Vector2 Origin = Vector2.Zero;

        public bool EditorLocked = false;
        public bool EditorCollapse = false;
        public int EditorBin = 0;
        public int EditorLayer = 0;

        public List<PositionKeyframe> PositionKeyframes = new List<PositionKeyframe>();
        public List<ScaleKeyframe> ScaleKeyframes = new List<ScaleKeyframe>();
        public List<RotationKeyframe> RotationKeyframes = new List<RotationKeyframe>();
        public List<ColorKeyframe> ColorKeyframes = new List<ColorKeyframe>();

        internal Prefab Prefab { get; }

        private string parentId = string.Empty;
        private HashSet<string> childrenIds = new HashSet<string>();

        internal PrefabObject(string name, string id, Prefab prefab)
        {
            Name = name;
            ID = id;
            Prefab = prefab;
        }

        /// <summary>
        /// Gets the object's parent.
        /// </summary>
        /// <returns>The object's parent instance or null if there is no parent.</returns>
        public PrefabObject GetParent()
        {
            return string.IsNullOrEmpty(parentId) ? null : Prefab.PrefabObjects[parentId];
        }

        /// <summary>
        /// Sets the object's parent.
        /// </summary>
        /// <param name="parent">The new parent. Pass null to unparent.</param>
        public void SetParent(PrefabObject parent)
        {
            if (parent.Prefab != Prefab)
            {
                throw new ArgumentException("Prefab of new parent does not match this object's prefab");
            }

            PrefabObject oldParent = GetParent();

            if (oldParent != null)
            {
                oldParent.childrenIds.Remove(ID);
            }

            if (parent != null)
            {
                parent.childrenIds.Add(ID);
                parentId = parent.ID;
            }
            else
            {
                parentId = string.Empty;
            }
        }

        /// <summary>
        /// Gets how many children this object has
        /// </summary>
        /// <returns>The children count.</returns>
        public int GetChildrenCount()
        {
            return childrenIds.Count;
        }

        /// <summary>
        /// Gets the list of children. This is an O(n) operation where n is the children count.
        /// </summary>
        /// <returns>The list of children.</returns>
        public List<PrefabObject> GetChildrenList()
        {
            List<PrefabObject> children = new List<PrefabObject>(GetChildrenCount());
            foreach (string childId in childrenIds)
            {
                children.Add(Prefab.PrefabObjects[childId]);
            }

            return children;
        }

        /// <summary>
        /// Add a child to this object.
        /// </summary>
        /// <param name="child">A prefab object.</param>
        public void AddChild(PrefabObject child)
        {
            if (child == null)
            {
                throw new ArgumentNullException("child was null");
            }

            if (child.Prefab != Prefab)
            {
                throw new ArgumentException("Prefab of child does not match this object's prefab");
            }

            child.SetParent(this);
        }

        public JSONNode ToJson(PrefabBuildFlags flags)
        {
            JSONObject json = new JSONObject();
            json["name"] = Name;
            json["id"] = ID;
            json["p"] = parentId;
            json["pt"] = (PositionParenting ? "1" : "0") + (ScaleParenting ? "1" : "0") + (RotationParenting ? "1" : "0");
            json["po"][0] = PositionParentOffset.ToString();
            json["po"][1] = ScaleParentOffset.ToString();
            json["po"][2] = RotationParentOffset.ToString();
            json["d"] = RenderDepth.ToString();
            json["ot"] = ((int)ObjectType).ToString();
            json["shape"] = ((int)Shape).ToString();
            json["so"] = ShapeOption.ToString();

            if (Shape == PrefabObjectShape.Text)
            {
                json["text"] = Text;
            }

            json["st"] = StartTime.ToString();
            json["akt"] = ((int)AutoKillType).ToString();
            json["ako"] = AutoKillOffset.ToString();
            json["o"]["x"] = Origin.X;
            json["o"]["y"] = Origin.Y;
            json["ed"]["locked"] = EditorLocked.ToString();
            json["ed"]["shrink"] = EditorCollapse.ToString();
            json["ed"]["bin"] = EditorBin.ToString();
            json["ed"]["layer"] = EditorLayer.ToString();

            if ((flags & PrefabBuildFlags.SortKeyframes) != 0)
            {
                PositionKeyframes.Sort((x, y) => x.Time.CompareTo(y.Time));
                ScaleKeyframes.Sort((x, y) => x.Time.CompareTo(y.Time));
                ColorKeyframes.Sort((x, y) => x.Time.CompareTo(y.Time));
            }

            if ((flags & (PrefabBuildFlags.AbsoluteRotation | PrefabBuildFlags.SortKeyframes)) != 0)
            {
                RotationKeyframes.Sort((x, y) => x.Time.CompareTo(y.Time));
            }

            json["events"]["pos"] = new JSONArray();
            for (int i = 0; i < PositionKeyframes.Count; i++)
            {
                JSONNode kfJson = json["events"]["pos"][i] = new JSONObject();
                PositionKeyframe kf = PositionKeyframes[i];

                kfJson["t"] = kf.Time.ToString();
                kfJson["x"] = kf.Value.X.ToString();
                kfJson["y"] = kf.Value.Y.ToString();
                kfJson["ct"] = kf.Easing.ToString();

                if (kf.RandomMode != PrefabObjectRandomMode.None)
                {
                    kfJson["r"] = ((int)kf.RandomMode).ToString();
                    kfJson["rx"] = kf.RandomValue.X.ToString();
                    kfJson["ry"] = kf.RandomValue.Y.ToString();
                    kfJson["rz"] = kf.RandomInterval.ToString();
                }
            }

            json["events"]["sca"] = new JSONArray();
            for (int i = 0; i < ScaleKeyframes.Count; i++)
            {
                JSONNode kfJson = json["events"]["sca"][i] = new JSONObject();
                ScaleKeyframe kf = ScaleKeyframes[i];

                kfJson["t"] = kf.Time.ToString();
                kfJson["x"] = kf.Value.X.ToString();
                kfJson["y"] = kf.Value.Y.ToString();
                kfJson["ct"] = kf.Easing.ToString();

                if (kf.RandomMode != PrefabObjectRandomMode.None)
                {
                    kfJson["r"] = ((int)kf.RandomMode).ToString();
                    kfJson["rx"] = kf.RandomValue.X.ToString();
                    kfJson["ry"] = kf.RandomValue.Y.ToString();
                    kfJson["rz"] = kf.RandomInterval.ToString();
                }
            }

            json["events"]["rot"] = new JSONArray();
            float currentRot = 0.0f;
            for (int i = 0; i < RotationKeyframes.Count; i++)
            {
                JSONNode kfJson = json["events"]["rot"][i] = new JSONObject();
                RotationKeyframe kf = RotationKeyframes[i];

                kfJson["t"] = kf.Time.ToString();
                kfJson["x"] = (kf.Value - currentRot).ToString();
                kfJson["ct"] = kf.Easing.ToString();

                if (kf.RandomMode != PrefabObjectRandomMode.None)
                {
                    kfJson["r"] = ((int)kf.RandomMode).ToString();
                    kfJson["rx"] = (kf.RandomValue - currentRot).ToString();
                    kfJson["rz"] = kf.RandomInterval.ToString();
                }

                if ((flags & PrefabBuildFlags.AbsoluteRotation) != 0)
                {
                    currentRot += kf.Value;
                }
            }

            json["events"]["col"] = new JSONArray();
            for (int i = 0; i < ColorKeyframes.Count; i++)
            {
                JSONNode kfJson = json["events"]["col"][i] = new JSONObject();
                ColorKeyframe kf = ColorKeyframes[i];

                kfJson["t"] = kf.Time.ToString();
                kfJson["x"] = kf.Value.ToString();
                kfJson["ct"] = kf.Easing.ToString();
            }

            return json;
        }
    }
}
