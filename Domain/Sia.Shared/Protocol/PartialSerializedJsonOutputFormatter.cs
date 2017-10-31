using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Buffers;
using System.Text;
using Sia.Shared.Data;

namespace Sia.Shared.Protocol
{
    public class PartialSerializedJsonOutputFormatter : JsonOutputFormatter
    {
        public PartialSerializedJsonOutputFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool)
            : base(serializerSettings, charPool)
        {

        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var dataStream = (IEnumerable<IJsonDataObject>)context.Object;

            foreach (var objectToWrite in dataStream)
            {
                var jsonData = objectToWrite.Data;
                if (jsonData is string) objectToWrite.Data = Deserialize((string)jsonData);
            }

            return base.WriteResponseBodyAsync(context, selectedEncoding);
        }

        private const int NumberOfCharactersInGenericTypeNotUsedByGetInterfaceMethod = 3;

        protected override bool CanWriteType(Type type)
        {
            if (type is null
                || !type.IsGenericType
                || type.GetGenericArguments().Count() != 1) return false;

            var enumIntName = typeof(IEnumerable<>).ToString();
            var enumerableInterface = type.GetInterface(enumIntName
                .Substring(0, enumIntName.Length - NumberOfCharactersInGenericTypeNotUsedByGetInterfaceMethod));
            if (enumerableInterface is null) return false;

            return !(type.GetGenericArguments()[0].GetInterface(nameof(IJsonDataObject)) is null);
        }

        private object Deserialize(string serializedData) => JsonConvert.DeserializeObject(serializedData);
    }
}
