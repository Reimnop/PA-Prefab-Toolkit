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
                    string xOrgStr = origin.Value<string>("x");
                    string yOrgStr = origin.Value<string>("y");

                    prefabObject.Origin.X = xOrgStr == "-0.5" ? PrefabObjectOriginX.Left : (xOrgStr == "0" ? PrefabObjectOriginX.Center : PrefabObjectOriginX.Right);
                    prefabObject.Origin.Y = yOrgStr == "-0.5" ? PrefabObjectOriginY.Bottom : (yOrgStr == "0" ? PrefabObjectOriginY.Center : PrefabObjectOriginY.Top);
                }

                JObject editor = obj.Value<JObject>("ed");
                if (editor != null)
                    prefabObject.Editor = serializer.Deserialize<PrefabObject.EditorData>(editor.CreateReader());

                JObject events = obj.Value<JObject>("events");
                prefabObject.Events = serializer.Deserialize<PrefabObject.ObjectEvents>(events.CreateReader());
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
                writer.WriteValue(prefabObject.Origin.X == PrefabObjectOriginX.Left ? "-0.5" : (prefabObject.Origin.X == PrefabObjectOriginX.Center ? "0" : "0.5"));
                writer.WritePropertyName("y");
                writer.WriteValue(prefabObject.Origin.Y == PrefabObjectOriginY.Bottom ? "-0.5" : (prefabObject.Origin.Y == PrefabObjectOriginY.Center ? "0" : "0.5"));
            }
            writer.WriteEndObject();

            //object editor data
            writer.WritePropertyName("ed");
            serializer.Serialize(writer, prefabObject.Editor);

            //object events
            writer.WritePropertyName("events");
            serializer.Serialize(writer, prefabObject.Events);

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
            return objectType == typeof(PrefabObject.ObjectEvents);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.ObjectEvents prefabEvents = new PrefabObject.ObjectEvents();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                JObject obj = (JObject)token;

                JArray positionArr = obj.Value<JArray>("pos");
                var positionKeyframes = new PrefabObject.ObjectEvents.PositionKeyframe[positionArr.Count];
                for (int i = 0; i < positionArr.Count; i++)
                    positionKeyframes[i] = serializer.Deserialize<PrefabObject.ObjectEvents.PositionKeyframe>(positionArr[i].CreateReader());
                prefabEvents.PositionKeyframes = new PrefabObject.ObjectEvents.KeyframeList<PrefabObject.ObjectEvents.PositionKeyframe>(positionKeyframes.ToList());

                JArray scaleArr = obj.Value<JArray>("sca");
                PrefabObject.ObjectEvents.ScaleKeyframe[] scaleKeyframes = new PrefabObject.ObjectEvents.ScaleKeyframe[scaleArr.Count];
                for (int i = 0; i < scaleArr.Count; i++)
                    scaleKeyframes[i] = serializer.Deserialize<PrefabObject.ObjectEvents.ScaleKeyframe>(scaleArr[i].CreateReader());
                prefabEvents.ScaleKeyframes = new PrefabObject.ObjectEvents.KeyframeList<PrefabObject.ObjectEvents.ScaleKeyframe>(scaleKeyframes.ToList());

                JArray rotationArr = obj.Value<JArray>("rot");
                PrefabObject.ObjectEvents.RotationKeyframe[] rotationKeyframes = new PrefabObject.ObjectEvents.RotationKeyframe[rotationArr.Count];
                for (int i = 0; i < rotationArr.Count; i++)
                    rotationKeyframes[i] = serializer.Deserialize<PrefabObject.ObjectEvents.RotationKeyframe>(rotationArr[i].CreateReader());
                prefabEvents.RotationKeyframes = new PrefabObject.ObjectEvents.KeyframeList<PrefabObject.ObjectEvents.RotationKeyframe>(rotationKeyframes.ToList());

                JArray colorArr = obj.Value<JArray>("col");
                PrefabObject.ObjectEvents.ColorKeyframe[] colorKeyframes = new PrefabObject.ObjectEvents.ColorKeyframe[colorArr.Count];
                for (int i = 0; i < colorArr.Count; i++)
                    colorKeyframes[i] = serializer.Deserialize<PrefabObject.ObjectEvents.ColorKeyframe>(colorArr[i].CreateReader());
                prefabEvents.ColorKeyframes = new PrefabObject.ObjectEvents.KeyframeList<PrefabObject.ObjectEvents.ColorKeyframe>(colorKeyframes.ToList());
            }

            return prefabEvents;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.ObjectEvents prefabEvents = (PrefabObject.ObjectEvents)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("pos");
            serializer.Serialize(writer, prefabEvents.PositionKeyframes);

            writer.WritePropertyName("sca");
            serializer.Serialize(writer, prefabEvents.ScaleKeyframes);

            writer.WritePropertyName("rot");
            serializer.Serialize(writer, prefabEvents.RotationKeyframes);

            writer.WritePropertyName("col");
            serializer.Serialize(writer, prefabEvents.ColorKeyframes);

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabPositionKeyframeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.ObjectEvents.PositionKeyframe);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.ObjectEvents.PositionKeyframe positionKeyframe = new PrefabObject.ObjectEvents.PositionKeyframe();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                positionKeyframe.Time = token.Value<float>("t");
                positionKeyframe.Value.X = token.Value<float>("x");
                positionKeyframe.Value.Y = token.Value<float>("y");

                string str = token.Value<string>("ct");
                if (!string.IsNullOrEmpty(str))
                    positionKeyframe.Easing = EaseStringConverter.StringToEase(str);

                int rnd = token.Value<int>("r");
                if (rnd != 0)
                {
                    positionKeyframe.RandomMode = (PrefabObjectRandomMode)rnd;
                    positionKeyframe.RandomValue.X = token.Value<float>("rx");
                    positionKeyframe.RandomValue.Y = token.Value<float>("ry");
                    positionKeyframe.RandomInterval = token.Value<float>("rz");
                }
            }

            return positionKeyframe;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.ObjectEvents.PositionKeyframe positionKeyframe = (PrefabObject.ObjectEvents.PositionKeyframe)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("t");
            writer.WriteValue(positionKeyframe.Time.ToString());

            writer.WritePropertyName("x");
            writer.WriteValue(positionKeyframe.Value.X.ToString());

            writer.WritePropertyName("y");
            writer.WriteValue(positionKeyframe.Value.Y.ToString());

            writer.WritePropertyName("ct");
            writer.WriteValue(EaseStringConverter.EaseToString(positionKeyframe.Easing));

            if (positionKeyframe.RandomMode != PrefabObjectRandomMode.None)
            {
                writer.WritePropertyName("r");
                writer.WriteValue(((int)positionKeyframe.RandomMode).ToString());

                writer.WritePropertyName("rx");
                writer.WriteValue(positionKeyframe.RandomValue.X.ToString());

                writer.WritePropertyName("ry");
                writer.WriteValue(positionKeyframe.RandomValue.Y.ToString());

                writer.WritePropertyName("rz");
                writer.WriteValue(positionKeyframe.RandomInterval.ToString());
            }

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabScaleKeyframeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.ObjectEvents.ScaleKeyframe);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.ObjectEvents.ScaleKeyframe scaleKeyframe = new PrefabObject.ObjectEvents.ScaleKeyframe();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                scaleKeyframe.Time = token.Value<float>("t");
                scaleKeyframe.Value.X = token.Value<float>("x");
                scaleKeyframe.Value.Y = token.Value<float>("y");

                string str = token.Value<string>("ct");
                if (!string.IsNullOrEmpty(str))
                    scaleKeyframe.Easing = EaseStringConverter.StringToEase(str);

                int rnd = token.Value<int>("r");
                if (rnd != 0)
                {
                    scaleKeyframe.RandomMode = (PrefabObjectRandomMode)rnd;
                    scaleKeyframe.RandomValue.X = token.Value<float>("rx");
                    scaleKeyframe.RandomValue.Y = token.Value<float>("ry");
                    scaleKeyframe.RandomInterval = token.Value<float>("rz");
                }
            }

            return scaleKeyframe;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.ObjectEvents.ScaleKeyframe scaleKeyframe = (PrefabObject.ObjectEvents.ScaleKeyframe)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("t");
            writer.WriteValue(scaleKeyframe.Time.ToString());

            writer.WritePropertyName("x");
            writer.WriteValue(scaleKeyframe.Value.X.ToString());

            writer.WritePropertyName("y");
            writer.WriteValue(scaleKeyframe.Value.Y.ToString());

            writer.WritePropertyName("ct");
            writer.WriteValue(EaseStringConverter.EaseToString(scaleKeyframe.Easing));

            if (scaleKeyframe.RandomMode != PrefabObjectRandomMode.None)
            {
                writer.WritePropertyName("r");
                writer.WriteValue(((int)scaleKeyframe.RandomMode).ToString());

                writer.WritePropertyName("rx");
                writer.WriteValue(scaleKeyframe.RandomValue.X.ToString());

                writer.WritePropertyName("ry");
                writer.WriteValue(scaleKeyframe.RandomValue.Y.ToString());

                writer.WritePropertyName("rz");
                writer.WriteValue(scaleKeyframe.RandomInterval.ToString());
            }

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabRotationKeyframeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.ObjectEvents.RotationKeyframe);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.ObjectEvents.RotationKeyframe rotationKeyframe = new PrefabObject.ObjectEvents.RotationKeyframe();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                rotationKeyframe.Time = token.Value<float>("t");
                rotationKeyframe.Value = token.Value<float>("x");

                string str = token.Value<string>("ct");
                if (!string.IsNullOrEmpty(str))
                    rotationKeyframe.Easing = EaseStringConverter.StringToEase(str);

                int rnd = token.Value<int>("r");
                if (rnd != 0)
                {
                    rotationKeyframe.RandomMode = (PrefabObjectRandomMode)rnd;
                    rotationKeyframe.RandomValue = token.Value<float>("rx");
                    rotationKeyframe.RandomInterval = token.Value<float>("rz");
                }
            }

            return rotationKeyframe;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.ObjectEvents.RotationKeyframe rotationKeyframe = (PrefabObject.ObjectEvents.RotationKeyframe)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("t");
            writer.WriteValue(rotationKeyframe.Time.ToString());

            writer.WritePropertyName("x");
            writer.WriteValue(rotationKeyframe.Value.ToString());

            writer.WritePropertyName("ct");
            writer.WriteValue(EaseStringConverter.EaseToString(rotationKeyframe.Easing));

            if (rotationKeyframe.RandomMode != PrefabObjectRandomMode.None)
            {
                writer.WritePropertyName("r");
                writer.WriteValue(((int)rotationKeyframe.RandomMode).ToString());

                writer.WritePropertyName("rx");
                writer.WriteValue(rotationKeyframe.RandomValue.ToString());

                writer.WritePropertyName("rz");
                writer.WriteValue(rotationKeyframe.RandomInterval.ToString());
            }

            //end the block
            writer.WriteEndObject();
        }
    }

    internal class PrefabColorKeyframeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.ObjectEvents.ColorKeyframe);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PrefabObject.ObjectEvents.ColorKeyframe colorKeyframe = new PrefabObject.ObjectEvents.ColorKeyframe();

            JToken token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Object)
            {
                colorKeyframe.Time = token.Value<float>("t");
                colorKeyframe.Value = token.Value<int>("x");

                string str = token.Value<string>("ct");
                if (!string.IsNullOrEmpty(str))
                    colorKeyframe.Easing = EaseStringConverter.StringToEase(str);
            }

            return colorKeyframe;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefabObject.ObjectEvents.ColorKeyframe colorKeyframe = (PrefabObject.ObjectEvents.ColorKeyframe)value;

            //begin a block
            writer.WriteStartObject();

            writer.WritePropertyName("t");
            writer.WriteValue(colorKeyframe.Time.ToString());

            writer.WritePropertyName("x");
            writer.WriteValue(colorKeyframe.Value.ToString());

            writer.WritePropertyName("ct");
            writer.WriteValue(EaseStringConverter.EaseToString(colorKeyframe.Easing));

            //end the block
            writer.WriteEndObject();
        }
    }
}
