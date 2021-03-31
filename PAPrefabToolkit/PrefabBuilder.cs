using Newtonsoft.Json;
using PAPrefabToolkit.Data;
using System.Diagnostics;

namespace PAPrefabToolkit
{
    /// <summary>
    /// Prefab building helper class.
    /// </summary>
    public static class PrefabBuilder
    {
        private static JsonConverter[] converters = new JsonConverter[]
        {
            new PrefabConverter(),
            new PrefabObjectConverter(),
            new PrefabEditorDataConverter(),
            new PrefabEventsConverter(),
            new PrefabPositionKeyframeConverter(),
            new PrefabScaleKeyframeConverter(),
            new PrefabRotationKeyframeConverter(),
            new PrefabColorKeyframeConverter()
        };

        /// <summary>
        /// Convert Prefab class into raw Prefab JSON.
        /// </summary>
        /// <param name="prefab">Prefab class</param>
        /// <param name="formatting">Output formatting</param>
        /// <returns>Raw prefab JSON</returns>
        public static string BuildPrefab(Prefab prefab, Formatting formatting = Formatting.None)
            => JsonConvert.SerializeObject(prefab, formatting, converters);

        /// <summary>
        /// Converts raw Prefab JSON into a Prefab class.
        /// </summary>
        /// <param name="prefabString">Raw prefab string</param>
        /// <returns>Prefab class</returns>
        public static Prefab ReadPrefab(string prefabString)
            => JsonConvert.DeserializeObject<Prefab>(prefabString, converters);
    }
}
