namespace WS_Multithread.Threading;

/// <summary>
/// Orchestre le lancement et l'attente d'un lot de threads.
/// </summary>
internal sealed class ThreadBatchRunner
{
    private readonly ThreadBatchOptions _options;
    private readonly IThreadScenario _scenario;

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="ThreadBatchRunner"/>.
    /// </summary>
    /// <param name="options">Configuration de lancement des threads.</param>
    /// <param name="scenario">Scenario execute par chaque thread.</param>
    public ThreadBatchRunner(ThreadBatchOptions options, IThreadScenario scenario)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _scenario = scenario ?? throw new ArgumentNullException(nameof(scenario));
    }

    /// <summary>
    /// Lance tous les threads puis attend leur fin d'execution.
    /// </summary>
    public void Run()
    {
        var threads = new List<Thread>(_options.ThreadCount);

        for (var index = 1; index <= _options.ThreadCount; index++)
        {
            var threadName = $"Thread_{index:D3}";
            var thread = new Thread(_scenario.Execute)
            {
                Name = threadName
            };

            threads.Add(thread);
            thread.Start(threadName);

            if (_options.DelayBetweenStartsMilliseconds > 0)
            {
                Thread.Sleep(_options.DelayBetweenStartsMilliseconds);
            }
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }
}