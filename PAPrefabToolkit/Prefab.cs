using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PAPrefabToolkit
{
    /// <summary>
    /// The prefab, the base of the library. Use this to make a prefab and export to a file or to a stream.
    /// </summary>
    public class Prefab : IEnumerable<PrefabObject>
    {
        private static Random random;

        static Prefab()
        {
            random = new Random();
        }

        private string GenId()
        {
            const string idChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~!@#$%^&*_+{}|:<>?,./;'[]▓▒░▐▆▉☰☱☲☳☴☵☶☷►▼◄▬▩▨▧▦▥▤▣▢□■¤ÿòèµ¶™ßÃ®¾ð¥œ⁕(◠‿◠✿)";

            string id = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                id += idChars[random.Next(0, idChars.Length)];
            }

            return id;
        }

        /// <summary>
        /// The prefab's name. This will be visible in the Project Arrhythmia Editor.
        /// </summary>
        public string Name;

        /// <summary>
        /// The prefab's type. This will be visible the in the Project Arrhythmia Editor.
        /// </summary>
        public PrefabType Type;

        /// <summary>
        /// The prefab's offset. I don't even know what this does but I'll put it here anyway.
        /// </summary>
        public float Offset;

        internal Dictionary<string, PrefabObject> PrefabObjects = new Dictionary<string, PrefabObject>();

        /// <summary>
        /// Default prefab constructor. Name is assigned to "Untitled" and Type is assigned to PrefabType.Bombs.
        /// </summary>
        public Prefab()
        {
            Name = "Untitled";
            Type = PrefabType.Bombs;
        }

        /// <summary>
        /// Prefab constructor.
        /// </summary>
        /// <param name="name">Prefab name</param>
        /// <param name="type">Prefab type</param>
        public Prefab(string name, PrefabType type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Construct a prefab with a json object.
        /// </summary>
        /// <param name="json"></param>
        public Prefab(JSONNode json)
        {
            Name = json["name"];
            Type = (PrefabType)json["type"].AsInt;
            Offset = json["offset"].AsFloat;

            JSONNode objectsArray = json["objects"];
            foreach (JSONNode objectJson in objectsArray)
            {
                PrefabObjects.Add(objectJson["id"], new PrefabObject(objectJson, this));
            }

            foreach (PrefabObject obj in PrefabObjects.Values)
            {
                PrefabObject parent = obj.GetParent();
                if (parent != null)
                {
                    parent.ChildrenIDs.Add(obj.ID);
                }
            }
        }

        /// <summary>
        /// Create an object and stores it the prefab.
        /// </summary>
        /// <returns>A new prefab object</returns>
        public PrefabObject CreateObject(string name)
        {
            string id = GenId();

            PrefabObject obj = new PrefabObject(name, id, this);
            PrefabObjects.Add(id, obj);

            return obj;
        }

        /// <summary>
        /// Unparents all object's children and deletes the object.
        /// </summary>
        /// <param name="prefabObject">An existing object</param>
        public void DeleteObject(PrefabObject prefabObject)
        {
            if (prefabObject.Prefab != this)
            {
                throw new ArgumentException("prefabObject does not belong to this prefab");
            }

            foreach (PrefabObject obj in prefabObject.GetChildrenList())
            {
                obj.SetParent(null);
            }

            PrefabObjects.Remove(prefabObject.ID);
        }

        /// <summary>
        /// Writes the prefab to a file.
        /// </summary>
        /// <param name="path">A file path</param>
        /// <param name="flags">Build flags</param>
        public void ExportToFile(string path, PrefabBuildFlags flags = PrefabBuildFlags.None)
        {
            File.WriteAllText(path, ToJson(flags).ToString());
        }

        /// <summary>
        /// Gets a JSON object of the prefab.
        /// </summary>
        /// <param name="flags">Configuration flags to configure the output of the prefab builder.</param>
        /// <returns>A JSONNode object</returns>
        public JSONNode ToJson(PrefabBuildFlags flags = PrefabBuildFlags.None)
        {
            JSONObject json = new JSONObject();
            json["name"] = Name;
            json["type"] = ((int)Type).ToString();
            json["offset"] = Offset.ToString();

            PrefabObject[] prefabObjects = new PrefabObject[PrefabObjects.Count];
            PrefabObjects.Values.CopyTo(prefabObjects, 0);

            if ((flags & PrefabBuildFlags.SortObjects) != 0) 
            {
                Array.Sort(prefabObjects, (x, y) => x.StartTime.CompareTo(y.StartTime));
            }

            foreach (PrefabObject obj in prefabObjects)
            {
                json["objects"].Add(obj.ToJson(flags));
            }

            return json;
        }

        public IEnumerator<PrefabObject> GetEnumerator()
        {
            return PrefabObjects.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
