using WS_Multithread.Threading;

namespace WS_Multithread;

/// <summary>
/// Point d'entree de la feature 1 du workshop multithreading.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Lance 300 threads executant <see cref="ObservationScenario.FctA(object?)"/>.
    /// </summary>
    private static void Main()
    {
        var options = new ThreadBatchOptions(
            threadCount: 300,
            delayBetweenStartsMilliseconds: 10,
            simulatedWorkDurationMilliseconds: 30);

        ObservationScenario.ResetCounter();
        IThreadScenario scenario = new ObservationScenario(options.SimulatedWorkDurationMilliseconds);
        var runner = new ThreadBatchRunner(options, scenario);

        runner.Run();

        Console.WriteLine();
        Console.WriteLine(
            $"Fin d'execution. Valeur finale de _nb_thread_in_progress = {ObservationScenario.CurrentInProgressCount}");
    }
}