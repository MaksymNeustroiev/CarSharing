using Amazon.SQS;
using Amazon.SQS.Model;
using CarSharing.Core.Entities;
using CarSharing.Core.Services;
using CarSharing.Infrastructure.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarSharing.Infrastructure.Services
{
    public class CarLockingService : ICarLockingService
    {
        private readonly IAmazonSQS amazonSQS;
        private static readonly string queueUrl = "https://sqs.eu-west-1.amazonaws.com";

        public CarLockingService(IAmazonSQS amazonSQS)
        {
            this.amazonSQS = amazonSQS;
        }

        public async Task LockCar(Car car)
        {
            var command = new DeviceCommand { Command = CommandType.Lock, Imei = car.Tracker.Imei };

            await SendMessage(command);
        }

        public async Task UnlockCar(Car car)
        {
            var command = new DeviceCommand { Command = CommandType.Unlock, Imei = car.Tracker.Imei };

            await SendMessage(command);
        }

        private async Task SendMessage(DeviceCommand command)
        {
            string jsonBody = JsonSerializer.Serialize(command);
            var request = new SendMessageRequest(queueUrl, jsonBody)
            {
                MessageGroupId = "CarSharing.Api",
                MessageDeduplicationId = command.GetHashCode().ToString()
            };
            await amazonSQS.SendMessageAsync(request);
        }
    }
}
