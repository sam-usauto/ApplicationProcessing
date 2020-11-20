using ApplicationProcessing.Service.ScoringSolution.Repositories;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Configuration;
using ApplicationProcessing.Service.TrustScienceService.Services;
using Common.DTOs.Application;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.BackgroundServices
{
    public class FetchReportsService : BackgroundService
    {
        private readonly TrustScienceConfiguration _config;
        private readonly ITrustScienceService _trustScienceService;
        private readonly ITrustScienceRepository _trustScienceRepository;

        private ApplicationStepInput _applicationStepInput = null;
        private int _dapperTimeOut = 90;
        private readonly int _refreshIntervalInSeconds;

        public FetchReportsService(TrustScienceConfiguration config,
                                    ITrustScienceRepository trustScienceRepository,
                                    ITrustScienceService trustScienceService)
        {
            _config = config;
            _trustScienceService = trustScienceService;
            _dapperTimeOut = _config.DapperDefaultTimeOut;
            _trustScienceRepository = trustScienceRepository;
            _refreshIntervalInSeconds =  60;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalInSeconds), stoppingToken);

                var applist = await _trustScienceService.FetchReportsFromTrustScience();

                //if (forecast is object)
                //{
                //    var currentWeather = new CurrentWeatherResult { Description = forecast.Weather.Description };

                //    var cacheKey = $"current_weather_{DateTime.UtcNow:yyyy_MM_dd}";

                //    _logger.LogInformation("Updating weather in cache.");

                //    await _cache.SetAsync(cacheKey, currentWeather, _minutesToCache);
                //}
            }
        }
    }
}
