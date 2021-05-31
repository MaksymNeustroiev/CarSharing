using Amazon.SQS;
using Amazon.SQS.Model;
using CarSharing.Api.Models;
using CarSharing.Application.UseCases.DeviceState;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CarSharing.Api.Events
{
    public class EventSubscriber : IHostedService
    {
        private readonly IAmazonSQS amazonSQS;
        private readonly IUpdateDeviceStateUseCase updateDeviceStateUseCase;

        private static readonly string queueUrl = "https://sqs.eu-west-1.amazonaws.com";

        public EventSubscriber(IAmazonSQS amazonSQS, IUpdateDeviceStateUseCase updateDeviceStateUseCase)
        {
            this.amazonSQS = amazonSQS;
            this.updateDeviceStateUseCase = updateDeviceStateUseCase;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var messages = await GetMessage();

                foreach (var message in messages.Messages)
                {
                    if (await ProcessMessage(message))
                    {
                        await DeleteMessage(message);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task<ReceiveMessageResponse> GetMessage()
        {
            return await amazonSQS.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 1,
                WaitTimeSeconds = 5
            });
        }


        private async Task<bool> ProcessMessage(Message message)
        {
            var deviceState = JsonSerializer.Deserialize<DeviceStateMessage>(message.Body);

            var input = new UpdateDeviceStateInput
            {
                Imei = deviceState.Imei,
                FuelRemainingFraction = deviceState.FuelRemainingFraction,
                Ignition = deviceState.Ignition,
                Locked = deviceState.Locked,
                MileageTotalKm = deviceState.MileageTotalKm,
                Position = deviceState.Position
            };

            await updateDeviceStateUseCase.HandleAsync(input);

            return true;
        }

        private async Task DeleteMessage(Message message)
        {
            await amazonSQS.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
        }

    }
}
