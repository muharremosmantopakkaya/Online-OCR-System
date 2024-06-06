using EkonLayer.Core.LogModels;
using EkonLayer.Core.Services;
using EkonLayer.Helpers.Models.Dtos.LogModelDtos;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Logging.Methods
{
    public class TimerWorker : BackgroundService
    {
        private bool timerProgress = false;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessLogsAsync();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
        private async Task ProcessLogsAsync()
        {
            if (!timerProgress)
            {
                timerProgress = true;

                try
                {

                }
                catch (Exception ex)
                {
                }

                timerProgress = false;
            }
        }
    }
}
