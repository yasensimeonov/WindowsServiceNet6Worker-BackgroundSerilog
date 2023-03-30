namespace WindowsService_Net6Worker_Background
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("The service has been stopped...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(1000, stoppingToken);

                var result = await client.GetAsync("https://www.corespider.com");
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("CoreSpider is running and the Status code {StatusCode}", result.StatusCode);
                }
                else
                {
                    _logger.LogError("CoreSpider is is down. Status code {StatusCode}", result.StatusCode);
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }

    }
}