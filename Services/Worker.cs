using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreBustronic.Controllers;
using AspNetCoreBustronic.Core;
using AspNetCoreBustronic.Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using StackExchange.Redis;

namespace AspNetCoreBustronic.Services
{
    public class Worker : BackgroundService
    {
        readonly ILogger<Worker> _logger;
        private ConnectionMultiplexer connection;

        public IServiceProvider _provider { get; }

        public Worker(IServiceProvider provider, ILogger<Worker> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            var configuration = ConfigurationOptions.Parse("localhost:6379");
            connection = ConnectionMultiplexer.Connect(configuration);

            await DoWork(stoppingToken);
        }

        private void OnNotification(object sender, NpgsqlNotificationEventArgs e)
        {
            throw new Exception();
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            using (var scope = _provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BustronicContext>();
                var movingData = MovingData.Create(context);
                var movingVehicleInfos = MovingVehicleInfo.Create(movingData);
                var computation = new Computation(movingData);
                var publisher = connection.GetSubscriber();

                publisher.Subscribe("message", (channel, message) =>
                {
                    _logger.LogWarning($"channel: {channel}, message: {message}");
                });

                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var sw = Stopwatch.StartNew();
                    var result = computation.compute(movingVehicleInfos);
                    sw.Stop();
                    _logger.LogInformation($"count = {movingVehicleInfos.Count}, elapsed = {sw.ElapsedMilliseconds}");

                    movingVehicleInfos = result.Where(e => e.Error.Equals(String.Empty)).ToList();
                    //movingData.Update(movingVehicleInfos);
                    var toClient = JsonSerializer.Serialize(new
                    {
                        @event = "message",
                        data = movingVehicleInfos.Select(e => e.ToClient())
                    });
                    publisher.Publish("channels", toClient);

                    var invalid = result.Where(e => !e.Error.Equals(String.Empty)).ToList();
                    if (invalid.Count > 0)
                    {
                        movingData.Remove(invalid);
                        movingData.Insert(invalid);
                        movingVehicleInfos = MovingVehicleInfo.Create(movingData);
                    }

                    await Task.Delay(1000, stoppingToken);
                }
            }

        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            if (connection != null)
            {
                connection.Close();
            }
            await Task.CompletedTask;
        }
    }
}