using WS_Multithread.Threading;

namespace WS_Multithread;

/// <summary>
/// Point d'entree de la feature 6 du workshop multithreading.
/// </summary>
internal static class Program
{
    private const int MaxExclusiveAccessWaitMilliseconds = 200;

    /// <summary>
    /// Lance 300 threads executant <see cref="ObservationScenario.FctA(object?)"/>.
    /// </summary>
    /// <param name="args">Arguments de ligne de commande.</param>
    /// <remarks>
    /// Argument supporte:
    /// <list type="bullet">
    /// <item><description><c>--mode semaphore|lock|monitor|mutex|none</c>.</description></item>
    /// </list>
    /// </remarks>
    private static void Main(string[] args)
    {
        var exclusiveMode = ExclusiveAccessModeParser.Parse(args);
        using var exclusiveAccessPrimitive = ExclusiveAccessPrimitiveFactory.Create(exclusiveMode);

        var options = new ThreadBatchOptions(
            threadCount: 300,
            delayBetweenStartsMilliseconds: 10,
            simulatedWorkDurationMilliseconds: 30);

        ObservationScenario.ResetCounter();
        ObservationScenario.ResetExclusiveAccessCounter();

        using var completionEvent = new CountdownEvent(options.ThreadCount);

        Console.WriteLine($"Mode d'acces exclusif: {exclusiveAccessPrimitive.Name}");

        IThreadScenario businessScenario = new ObservationScenario(
            options.SimulatedWorkDurationMilliseconds,
            exclusiveAccessPrimitive,
            MaxExclusiveAccessWaitMilliseconds);
        IThreadScenario scenario = new CountdownSignalingScenarioDecorator(
            businessScenario,
            completionEvent);
        var runner = new ThreadBatchRunner(options, scenario);

        runner.Run();
        completionEvent.Wait();

        Console.WriteLine();
        Console.WriteLine(
            $"Fin d'execution. Valeur finale de _nb_thread_in_progress = {ObservationScenario.CurrentInProgressCount}");
        Console.WriteLine(
            $"Fin d'execution. Valeur finale de _CountExclusive_access = {ObservationScenario.CurrentExclusiveAccessCount}");
        Console.WriteLine(
            $"Fin d'execution. Valeur max observee de _CountExclusive_access = {ObservationScenario.CurrentMaxExclusiveAccessCount}");
        Console.WriteLine(
            $"Fin d'execution. Nombre de threads impatients (attente > {MaxExclusiveAccessWaitMilliseconds} ms) = {ObservationScenario.CurrentSkippedExclusiveAccessCount}");
    }
}
