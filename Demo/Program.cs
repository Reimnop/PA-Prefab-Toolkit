using System;
using PAPrefabParser;
using PAPrefabParser.Data;
using Newtonsoft.Json;
using System.IO;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Prefab prefab = new Prefab();
            prefab.Name = "Hello, World!";
            prefab.Objects = new System.Collections.Generic.List<PrefabObject>();

            PrefabObject prefabObject = new PrefabObject();
            prefabObject.Id = "aaaaaaaaaaaaaaaa";
            prefabObject.Name = "dsds";

            prefab.Objects.Add(prefabObject);

            File.WriteAllText("prefab.json", JsonConvert.SerializeObject(prefab, Formatting.Indented, 
                new PrefabConverter(),
                new PrefabObjectConverter(),
                new PrefabEditorDataConverter(),
                new PrefabEventsConverter(),
                new PrefabPositionEventConverter(),
                new PrefabScaleEventConverter(),
                new PrefabRotationEventConverter(),
                new PrefabColorEventConverter()
                ));
        }
    }
}
