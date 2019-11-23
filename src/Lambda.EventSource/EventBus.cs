using Amazon;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lambda.EventSource
{
    public class EventBus
    {
        private readonly IAmazonKinesis client;

        public EventBus()
        {
            var config = new AmazonKinesisConfig
            {
                RegionEndpoint = RegionEndpoint.USEast1,
            };

             client = new AmazonKinesisClient(config);
        }

        public async Task PublishMessage<TMessage>(TMessage message, string partitionKey, string streamName)
        {
            byte[] buffer = JsonSerializer.SerializeToUtf8Bytes(message);
            using (var memoryStream = new MemoryStream(buffer))
            {
                var request = new PutRecordRequest
                {
                    Data = memoryStream,
                    PartitionKey = partitionKey,
                    StreamName = streamName,
                };

                PutRecordResponse response = await client.PutRecordAsync(request);
            }
        }
    }
}
