using EkonLayer.Core.Services;
using EkonLayer.Helpers.Models.Dtos.DbModelDtos;
using EkonLayer.Helpers.Models.Dtos.LogModelDtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EkonLayer.Logging.Methods
{
    public class LogWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<ApplicationDto> _options;

        private bool logProgress = false;
        private readonly ConcurrentQueue<ApplicationLogDto> _applicationLog = new ConcurrentQueue<ApplicationLogDto>();
        private readonly ConcurrentQueue<ErrorLogDto> _errorLog = new ConcurrentQueue<ErrorLogDto>();
        private readonly ConcurrentQueue<UserLogDto> _userLog = new ConcurrentQueue<UserLogDto>();

        public LogWorker(IServiceProvider serviceProvider, IOptions<ApplicationDto> options)
        {
            _serviceProvider = serviceProvider;
            _options = options;
        }

        public void AddApplicationLogEnqueue(ApplicationLogDto item)
        {
            _applicationLog.Enqueue(item);
        }

        public void AddErrorLogEnqueue(ErrorLogDto item)
        {
            _errorLog.Enqueue(item);
        }

        public void AddUserLogEnqueue(UserLogDto item)
        {
            _userLog.Enqueue(item);
        }

        private async Task ProcessLogsAsync()
        {
            if (!logProgress)
            {
                logProgress = true;

                try
                {
                    var applicationLogs = DequeueLogs(_applicationLog);
                    var userLogs = DequeueLogs(_userLog);
                    var errorLogs = DequeueLogs(_errorLog);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var applicationLogService = scope.ServiceProvider.GetRequiredService<IApplicationLogService>();
                        var errorLogService = scope.ServiceProvider.GetRequiredService<IErrorLogService>();
                        var userLogService = scope.ServiceProvider.GetRequiredService<IUserLogService>();

                        if (applicationLogs.Count > 0)
                        {
                            await applicationLogService.ApplicationLogWithBulk(applicationLogs);
                        }

                        if (errorLogs.Count > 0)
                        {
                            await errorLogService.ErrorLogWithBulk(errorLogs);
                        }

                        if (userLogs.Count > 0)
                        {
                            await userLogService.UserLogWithBulk(userLogs);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _errorLog.Enqueue(new ErrorLogDto() { Application = _options.Value.Name, Message = ex.Message, StackTrace = ex.StackTrace });
                }

                logProgress = false;
            }
        }

        private List<T> DequeueLogs<T>(ConcurrentQueue<T> queue)
        {
            var logs = new List<T>();
            while (queue.TryDequeue(out var log))
            {
                logs.Add(log);
                if (logs.Count >= 100)
                {
                    break;
                }
            }
            return logs;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessLogsAsync();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
