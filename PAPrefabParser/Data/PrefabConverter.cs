using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAPrefabParser.Data
{
    public class PrefabConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Prefab);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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

    public class PrefabObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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
            writer.WriteValue(prefabObject.ParentOffset[0].ToString());
            writer.WriteValue(prefabObject.ParentOffset[1].ToString());
            writer.WriteValue(prefabObject.ParentOffset[2].ToString());
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

    public class PrefabEditorDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.EditorData);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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

    public class PrefabEventsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.Events);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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

    public class PrefabPositionEventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.Events.PositionEvent);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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

            //end the block
            writer.WriteEndObject();
        }
    }

    public class PrefabScaleEventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.Events.ScaleEvent);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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

            //end the block
            writer.WriteEndObject();
        }
    }

    public class PrefabRotationEventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.Events.RotationEvent);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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

            //end the block
            writer.WriteEndObject();
        }
    }

    public class PrefabColorEventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrefabObject.Events.ColorEvent);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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

            //end the block
            writer.WriteEndObject();
        }
    }
}
