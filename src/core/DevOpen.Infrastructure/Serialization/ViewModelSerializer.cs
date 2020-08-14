using System;
using System.Globalization;
using System.IO;
using System.Text;
using DevOpen.Infrastructure.Serialization.Resolvers;
using Newtonsoft.Json;

namespace DevOpen.Infrastructure.Serialization
{
    public class ViewModelSerializer
    {
        private readonly JsonSerializer _serializer;
        
        public ViewModelSerializer()
        {
            _serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                ContractResolver = new EventJsonContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public string Serialize(object value)
        {
            var sb = new StringBuilder(256);
            var sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                _serializer.Serialize(jsonWriter, value, value.GetType());
            }

            return sw.ToString();
        }

        public T Deserialize<T>(string value)
        {
            return (T)Deserialize(value, typeof(T));
        }
        
        private object Deserialize(string value, Type type)
        {
            using (var reader = new JsonTextReader(new StringReader(value)))
            {
                return _serializer.Deserialize(reader, type);
            }
        }
    }
}