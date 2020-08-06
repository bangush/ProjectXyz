using System.IO;
using System.Text;
using Newtonsoft.Json;
using ProjectXyz.Api.Data.Serialization;

namespace ProjectXyz.Plugins.Data.Newtonsoft
{
    public sealed class Serializer : ISerializer
    {
        private readonly ISerializableConverterFacade _serializableConverterFacade;
        private readonly JsonSerializer _jsonSerializer;

        public Serializer(ISerializableConverterFacade serializableConverterFacade)
        {
            _serializableConverterFacade = serializableConverterFacade;
            _jsonSerializer = new JsonSerializer();
        }

        public void Serialize<TSerializable>(
            Stream stream,
            TSerializable serializable)
        {
            var dtoData = _serializableConverterFacade.ConvertBack(
                serializable,
                out var serializableId);
            var dto = new SerializableDto(
                serializableId,
                dtoData);

            using (var writer = new JsonTextWriter(new StreamWriter(stream, Encoding.UTF8, 4096, true)))
            {
                _jsonSerializer.Serialize(writer, dto);
            }
        }
    }
}