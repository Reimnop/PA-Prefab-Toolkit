using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAPrefabToolkit.Data
{
    public static class EaseStringConverter
    {
        private static Dictionary<PrefabObjectEasing, string> easingsToString = new Dictionary<PrefabObjectEasing, string>()
        {
            { PrefabObjectEasing.Linear, "Linear" },
            { PrefabObjectEasing.Instant, "Instant" },
            { PrefabObjectEasing.InSine, "InSine" },
            { PrefabObjectEasing.OutSine, "OutSine" },
            { PrefabObjectEasing.InOutSine, "InOutSine" },
            { PrefabObjectEasing.InElastic, "InElastic" },
            { PrefabObjectEasing.OutElastic, "OutElastic" },
            { PrefabObjectEasing.InOutElastic, "InOutElastic" },
            { PrefabObjectEasing.InBack, "InBack" },
            { PrefabObjectEasing.OutBack, "OutBack" },
            { PrefabObjectEasing.InOutBack, "InOutBack" },
            { PrefabObjectEasing.InBounce, "InBounce" },
            { PrefabObjectEasing.OutBounce, "OutBounce" },
            { PrefabObjectEasing.InOutBounce, "InOutBounce" },
            { PrefabObjectEasing.InQuad, "InQuad" },
            { PrefabObjectEasing.OutQuad, "OutQuad" },
            { PrefabObjectEasing.InOutQuad, "InOutQuad" },
            { PrefabObjectEasing.InCirc, "InCirc" },
            { PrefabObjectEasing.OutCirc, "OutCirc" },
            { PrefabObjectEasing.InOutCirc, "InOutCirc" },
            { PrefabObjectEasing.InExpo, "InExpo" },
            { PrefabObjectEasing.OutExpo, "OutExpo" },
            { PrefabObjectEasing.InOutExpo, "InOutExpo" }
        };
        private static Dictionary<string, PrefabObjectEasing> stringToEasings = new Dictionary<string, PrefabObjectEasing>()
        {
            { "Linear", PrefabObjectEasing.Linear },
            { "Instant", PrefabObjectEasing.Instant },
            { "InSine", PrefabObjectEasing.InSine },
            { "OutSine", PrefabObjectEasing.OutSine },
            { "InOutSine", PrefabObjectEasing.InOutSine },
            { "InElastic", PrefabObjectEasing.InElastic },
            { "OutElastic", PrefabObjectEasing.OutElastic },
            { "InOutElastic", PrefabObjectEasing.InOutElastic },
            { "InBack", PrefabObjectEasing.InBack },
            { "OutBack", PrefabObjectEasing.OutBack },
            { "InOutBack", PrefabObjectEasing.InOutBack },
            { "InBounce", PrefabObjectEasing.InBounce },
            { "OutBounce", PrefabObjectEasing.OutBounce },
            { "InOutBounce", PrefabObjectEasing.InOutBounce },
            { "InQuad", PrefabObjectEasing.InQuad },
            { "OutQuad", PrefabObjectEasing.OutQuad },
            { "InOutQuad", PrefabObjectEasing.InOutQuad },
            { "InCirc", PrefabObjectEasing.InCirc },
            { "OutCirc", PrefabObjectEasing.OutCirc },
            { "InOutCirc", PrefabObjectEasing.InOutCirc },
            { "InExpo", PrefabObjectEasing.InExpo },
            { "OutExpo", PrefabObjectEasing.OutExpo },
            { "InOutExpo", PrefabObjectEasing.InOutExpo }
        };

        public static string EaseToString(PrefabObjectEasing easing)
        {
            return easingsToString[easing];
        }

        public static PrefabObjectEasing StringToEase(string easingStr)
        {
            return stringToEasings[easingStr];
        }
    }

    internal class PrefabConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Prefab);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Prefab prefab = new Prefab();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                //read prefab metadata
                prefab.Name = obj.Value<string>("name");
                prefab.Type = (PrefabType)obj.Value<int>("type");
                prefab.Offset = obj.Value<float>("offset");

                //read prefab objects
                JArray arr = obj.Value<JArray>("objects");
                PrefabObject[] objects = new PrefabObject[arr.Count];
                for (int i = 0; i < arr.Count; i++)
                {
                    objects[i] = serializer.Deserialize<PrefabObject>(arr[i].CreateReader());
                }
                prefab.Objects = objects.ToList();
            }

            return prefab;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Prefab prefab = (Prefab)value;

            //begin prefab block
            writer.WriteStartObject();

            //prefab name
            writer.WritePropertyName("name");
            writer.WriteValue(prefab.Name);

            //prefab type
            writer.WritePropertyName("type");
            writer.WriteValue(((int)prefab.Type).ToString());

            //prefab offset
            writer.WritePropertyName("offset");
            writer.WriteValue(prefab.Offset.ToString());

            //prefab objects
            writer.WritePropertyName("objects");
            serializer.Serialize(writer, prefab.Objects);

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject prefabObject = new PrefabObject();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                prefabObject.Id = obj.Value<string>("id");

                string ptStr = obj.Value<string>("pt");
                if (!string.IsNullOrEmpty(ptStr))
                {
                    prefabObject.ParentType.PositionParenting = ptStr[0] != '0';
                    prefabObject.ParentType.ScaleParenting = ptStr[1] != '0';
                    prefabObject.ParentType.RotationParenting = ptStr[2] != '0';
                }

                JArray arr = obj.Value<JArray>("po");
                if (arr != null)
                {
                    prefabObject.ParentOffset.PositionOffset = arr[0].Value<float>();
                    prefabObject.ParentOffset.ScaleOffset = arr[1].Value<float>();
                    prefabObject.ParentOffset.RotationOffset = arr[2].Value<float>();
                }

                prefabObject.ParentId = obj.Value<string>("p");
                prefabObject.Depth = obj.Value<int>("d");
                prefabObject.ObjectType = (PrefabObjectType)obj.Value<int>("ot");
                prefabObject.StartTime = obj.Value<float>("st");
                prefabObject.Name = obj.Value<string>("name");
                prefabObject.Shape = (PrefabObjectShape)obj.Value<int>("shape");
                prefabObject.AutoKillType = (PrefabObjectAutoKillType)obj.Value<int>("akt");
                prefabObject.AutoKillOffset = obj.Value<float>("ako");
                prefabObject.ShapeOption = obj.Value<int>("so");

                JObject origin = obj.Value<JObject>("o");
                if (origin != null)
                {
                    prefabObject.Origin.X = origin.Value<float>("x");
                    prefabObject.Origin.Y = origin.Value<float>("y");
                }

                JObject editor = obj.Value<JObject>("ed");
                if (editor != null)
                    prefabObject.Editor = serializer.Deserialize<PrefabObject.EditorData>(editor.CreateReader());

                JObject events = obj.Value<JObject>("events");
                prefabObject.ObjectEvents = serializer.Deserialize<PrefabObject.Events>(events.CreateReader());
            }
            return prefabObject;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject prefabObject = (PrefabObject)value;

            //begin a block
            writer.WriteStartObject();

            //object id
            writer.WritePropertyName("id");
            writer.WriteValue(prefabObject.Id ?? throw new NullReferenceException("Object Id was null!"));

            //object parent type
            writer.WritePropertyName("pt");
            writer.WriteValue(
                (prefabObject.ParentType.PositionParenting ? "1" : "0") +
                (prefabObject.ParentType.ScaleParenting ? "1" : "0") +
                (prefabObject.ParentType.RotationParenting ? "1" : "0"));

            //object parent offset
            writer.WritePropertyName("po");
            writer.WriteStartArray();
            writer.WriteValue(prefabObject.ParentOffset.PositionOffset.ToString());
            writer.WriteValue(prefabObject.ParentOffset.ScaleOffset.ToString());
            writer.WriteValue(prefabObject.ParentOffset.RotationOffset.ToString());
            writer.WriteEndArray();

            //object parent id
            writer.WritePropertyName("p");
            writer.WriteValue(prefabObject.ParentId ?? string.Empty);

            //object depth
            writer.WritePropertyName("d");
            writer.WriteValue(prefabObject.Depth.ToString());

            //object type
            writer.WritePropertyName("ot");
            writer.WriteValue((int)prefabObject.ObjectType);

            //object start time
            writer.WritePropertyName("st");
            writer.WriteValue(prefabObject.StartTime.ToString());

            //object text
            if (prefabObject.Shape == PrefabObjectShape.Text)
            {
                writer.WritePropertyName("text");
                writer.WriteValue(prefabObject.Text ?? throw new NullReferenceException("Text was null!"));
            }

            //object name
            writer.WritePropertyName("name");
            writer.WriteValue(prefabObject.Name ?? throw new NullReferenceException("Object name was null!"));

            //object shape
            writer.WritePropertyName("shape");
            writer.WriteValue(((int)prefabObject.Shape).ToString());

            //object autokill
            writer.WritePropertyName("akt");
            writer.WriteValue((int)prefabObject.AutoKillType);

            writer.WritePropertyName("ako");
            writer.WriteValue(prefabObject.AutoKillOffset);

            //object shape option
            writer.WritePropertyName("so");
            writer.WriteValue(prefabObject.ShapeOption.ToString());

            //object origin
            writer.WritePropertyName("o");
            writer.WriteStartObject();
            {
                writer.WritePropertyName("x");
                writer.WriteValue(prefabObject.Origin.X.ToString());
                writer.WritePropertyName("y");
                writer.WriteValue(prefabObject.Origin.Y.ToString());
            }
            writer.WriteEndObject();

            //object editor data
            writer.WritePropertyName("ed");
            serializer.Serialize(writer, prefabObject.Editor);

            //object events
            writer.WritePropertyName("events");
            serializer.Serialize(writer, prefabObject.ObjectEvents);

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabEditorDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.EditorData);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.EditorData editor = new PrefabObject.EditorData();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                JObject obj = (JObject)token;
                editor.Locked = obj.Value<bool>("locked");
                editor.Collapse = obj.Value<bool>("shrink");
                editor.Bin = obj.Value<int>("bin");
                editor.Layer = obj.Value<int>("layer");
            }

            return editor;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.EditorData editorData = (PrefabObject.EditorData)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("locked");
            writer.WriteValue(editorData.Locked.ToString());
            writer.WritePropertyName("shrink");
            writer.WriteValue(editorData.Collapse.ToString());
            writer.WritePropertyName("bin");
            writer.WriteValue(editorData.Bin.ToString());
            writer.WritePropertyName("layer");
            writer.WriteValue(editorData.Layer.ToString());

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabEventsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.Events);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.Events prefabEvents = new PrefabObject.Events();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                JObject obj = (JObject)token;

                JArray positionArr = obj.Value<JArray>("pos");
                PrefabObject.Events.PositionEvent[] positionEvents = new PrefabObject.Events.PositionEvent[positionArr.Count];
                for (int i = 0; i < positionArr.Count; i++)
                    positionEvents[i] = serializer.Deserialize<PrefabObject.Events.PositionEvent>(positionArr[i].CreateReader());
                prefabEvents.PositionEvents = positionEvents.ToList();

                JArray scaleArr = obj.Value<JArray>("sca");
                PrefabObject.Events.ScaleEvent[] scaleEvents = new PrefabObject.Events.ScaleEvent[scaleArr.Count];
                for (int i = 0; i < scaleArr.Count; i++)
                    scaleEvents[i] = serializer.Deserialize<PrefabObject.Events.ScaleEvent>(scaleArr[i].CreateReader());
                prefabEvents.ScaleEvents = scaleEvents.ToList();

                JArray rotationArr = obj.Value<JArray>("rot");
                PrefabObject.Events.RotationEvent[] rotationEvents = new PrefabObject.Events.RotationEvent[rotationArr.Count];
                for (int i = 0; i < rotationArr.Count; i++)
                    rotationEvents[i] = serializer.Deserialize<PrefabObject.Events.RotationEvent>(rotationArr[i].CreateReader());
                prefabEvents.RotationEvents = rotationEvents.ToList();

                JArray colorArr = obj.Value<JArray>("col");
                PrefabObject.Events.ColorEvent[] colorEvents = new PrefabObject.Events.ColorEvent[colorArr.Count];
                for (int i = 0; i < colorArr.Count; i++)
                    colorEvents[i] = serializer.Deserialize<PrefabObject.Events.ColorEvent>(colorArr[i].CreateReader());
                prefabEvents.ColorEvents = colorEvents.ToList();
            }

            return prefabEvents;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.Events prefabEvents = (PrefabObject.Events)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("pos");
            serializer.Serialize(writer, prefabEvents.PositionEvents);

            writer.WritePropertyName("sca");
            serializer.Serialize(writer, prefabEvents.ScaleEvents);

            writer.WritePropertyName("rot");
            serializer.Serialize(writer, prefabEvents.RotationEvents);

            writer.WritePropertyName("col");
            serializer.Serialize(writer, prefabEvents.ColorEvents);

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabPositionEventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.Events.PositionEvent);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.Events.PositionEvent positionEvent = new PrefabObject.Events.PositionEvent();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                JObject obj = (JObject)token;
                positionEvent.Time = token.Value<float>("t");
                positionEvent.X = token.Value<float>("x");
                positionEvent.Y = token.Value<float>("y");

                string str = token.Value<string>("ct");
                if (!string.IsNullOrEmpty(str))
                    positionEvent.CurveType = EaseStringConverter.StringToEase(str);
            }

            return positionEvent;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.Events.PositionEvent positionEvent = (PrefabObject.Events.PositionEvent)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("t");
            writer.WriteValue(positionEvent.Time.ToString());

            writer.WritePropertyName("x");
            writer.WriteValue(positionEvent.X.ToString());

            writer.WritePropertyName("y");
            writer.WriteValue(positionEvent.Y.ToString());

            writer.WritePropertyName("ct");
            writer.WriteValue(EaseStringConverter.EaseToString(positionEvent.CurveType));

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabScaleEventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.Events.ScaleEvent);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.Events.ScaleEvent scaleEvent = new PrefabObject.Events.ScaleEvent();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                JObject obj = (JObject)token;
                scaleEvent.Time = token.Value<float>("t");
                scaleEvent.X = token.Value<float>("x");
                scaleEvent.Y = token.Value<float>("y");

                string str = token.Value<string>("ct");
                if (!string.IsNullOrEmpty(str))
                    scaleEvent.CurveType = EaseStringConverter.StringToEase(str);
            }

            return scaleEvent;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.Events.ScaleEvent scaleEvent = (PrefabObject.Events.ScaleEvent)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("t");
            writer.WriteValue(scaleEvent.Time.ToString());

            writer.WritePropertyName("x");
            writer.WriteValue(scaleEvent.X.ToString());

            writer.WritePropertyName("y");
            writer.WriteValue(scaleEvent.Y.ToString());

            writer.WritePropertyName("ct");
            writer.WriteValue(EaseStringConverter.EaseToString(scaleEvent.CurveType));

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabRotationEventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.Events.RotationEvent);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.Events.RotationEvent rotationEvent = new PrefabObject.Events.RotationEvent();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                JObject obj = (JObject)token;
                rotationEvent.Time = token.Value<float>("t");
                rotationEvent.X = token.Value<float>("x");

                string str = token.Value<string>("ct");
                if (!string.IsNullOrEmpty(str))
                    rotationEvent.CurveType = EaseStringConverter.StringToEase(str);
            }

            return rotationEvent;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.Events.RotationEvent rotationEvent = (PrefabObject.Events.RotationEvent)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("t");
            writer.WriteValue(rotationEvent.Time.ToString());

            writer.WritePropertyName("x");
            writer.WriteValue(rotationEvent.X.ToString());

            writer.WritePropertyName("ct");
            writer.WriteValue(EaseStringConverter.EaseToString(rotationEvent.CurveType));

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabColorEventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.Events.ColorEvent);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.Events.ColorEvent colorEvent = new PrefabObject.Events.ColorEvent();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                JObject obj = (JObject)token;
                colorEvent.Time = token.Value<float>("t");
                colorEvent.X = token.Value<float>("x");

                string str = token.Value<string>("ct");
                if (!string.IsNullOrEmpty(str))
                    colorEvent.CurveType = EaseStringConverter.StringToEase(str);
            }

            return colorEvent;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.Events.ColorEvent colorEvent = (PrefabObject.Events.ColorEvent)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("t");
            writer.WriteValue(colorEvent.Time.ToString());

            writer.WritePropertyName("x");
            writer.WriteValue(colorEvent.X.ToString());

            writer.WritePropertyName("ct");
            writer.WriteValue(EaseStringConverter.EaseToString(colorEvent.CurveType));

            //end the block
            writer.WriteEndObject();
        }
    }
}
