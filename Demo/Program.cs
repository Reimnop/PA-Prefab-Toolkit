using System;
using PAPrefabToolkit;
using PAPrefabToolkit.Data;
using Newtonsoft.Json;
using System.IO;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Prefab prefab = (Prefab)JsonConvert.DeserializeObject(File.ReadAllText("prefab.json"),
                typeof(Prefab),
                new PrefabConverter(),
                new PrefabObjectConverter(),
                new PrefabEditorDataConverter(),
                new PrefabEventsConverter(),
                new PrefabPositionEventConverter(),
                new PrefabScaleEventConverter(),
                new PrefabRotationEventConverter(),
                new PrefabColorEventConverter());

            File.WriteAllText("thing.lsp", JsonConvert.SerializeObject(prefab, Formatting.Indented, 
                new PrefabConverter(),
                new PrefabObjectConverter(),
                new PrefabEditorDataConverter(),
                new PrefabEventsConverter(),
                new PrefabPositionEventConverter(),
                new PrefabScaleEventConverter(),
                new PrefabRotationEventConverter(),
                new PrefabColorEventConverter()));
        }
    }
}
