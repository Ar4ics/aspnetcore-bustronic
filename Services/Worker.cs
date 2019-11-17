using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace AspNetCoreBustronic.Services
{
    public class Worker : BackgroundService
    {
        readonly ILogger<Worker> _logger;
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

            await DoWork(stoppingToken);
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

                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var sw = Stopwatch.StartNew();
                    movingVehicleInfos = computation.compute(movingVehicleInfos);
                    sw.Stop();
                    _logger.LogInformation($"count = {movingVehicleInfos.Count}, elapsed = {sw.ElapsedMilliseconds}");

                    await Task.Delay(3000, stoppingToken);
                }
            }

        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await Task.CompletedTask;
        }
    }
}