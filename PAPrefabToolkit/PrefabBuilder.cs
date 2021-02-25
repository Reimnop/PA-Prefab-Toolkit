using Newtonsoft.Json;
using PAPrefabToolkit.Data;
using System.Diagnostics;

namespace PAPrefabToolkit
{
    public static class PrefabBuilder
    {
        private static JsonConverter[] converters = new JsonConverter[]
        {
            new PrefabConverter(),
            new PrefabObjectConverter(),
            new PrefabEditorDataConverter(),
            new PrefabEventsConverter(),
            new PrefabPositionEventConverter(),
            new PrefabScaleEventConverter(),
            new PrefabRotationEventConverter(),
            new PrefabColorEventConverter()
        };

        /// <summary>
        /// Convert Prefab class into raw Prefab JSON.
        /// </summary>
        /// <param name="prefab">Prefab class</param>
        /// <param name="formatting">Output formatting</param>
        /// <returns>Raw prefab JSON</returns>
        public static string BuildPrefab(Prefab prefab, Formatting formatting = Formatting.None, bool noValidate = false)
        {
            if (!noValidate)
            {
                PrefabValidator validator = new PrefabValidator(prefab);
                validator.Validate();
            }
            else
                Debug.WriteLine("Warning: Prefab validation is disabled. May cause unexpected behaviour in-game.");

            return JsonConvert.SerializeObject(prefab, formatting, converters);
        }

        /// <summary>
        /// Converts raw Prefab JSON into a Prefab class.
        /// </summary>
        /// <param name="prefabString">Raw prefab string</param>
        /// <returns>Prefab class</returns>
        public static Prefab ReadPrefab(string prefabString)
        {
            return JsonConvert.DeserializeObject<Prefab>(prefabString, converters);
        }
    }
}
