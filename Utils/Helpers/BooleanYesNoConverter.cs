using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Arta.Subscriptions.Api.ArtaInfra.Utils.Helpers
{
    public class BooleanYesNoConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((value as bool?).HasValue && (value as bool?).Value ? "Yes" : "No");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().ToLower().Trim();

            if (value == null || String.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(String) || objectType == typeof(Boolean))
            {
                return true;
            }
            return false;
        }
    }
}
