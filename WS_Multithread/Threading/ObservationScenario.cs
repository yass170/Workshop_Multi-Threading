namespace WS_Multithread.Threading;

/// <summary>
/// Implemente la partie Observation du sujet.
/// </summary>
internal sealed class ObservationScenario : IThreadScenario
{
    /// <summary>
    /// Compteur partage de threads en cours, volontairement non protege pour observer les races.
    /// </summary>
    private static int _nb_thread_in_progress = 0;

    private static int _simulatedWorkDurationMilliseconds = 30;

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="ObservationScenario"/>.
    /// </summary>
    /// <param name="simulatedWorkDurationMilliseconds">Delai simule dans la methode FctA.</param>
    public ObservationScenario(int simulatedWorkDurationMilliseconds)
    {
        if (simulatedWorkDurationMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(simulatedWorkDurationMilliseconds),
                "Le delai de travail simule ne peut pas etre negatif.");
        }

        _simulatedWorkDurationMilliseconds = simulatedWorkDurationMilliseconds;
    }

    /// <summary>
    /// Obtient la valeur courante du compteur partage.
    /// </summary>
    public static int CurrentInProgressCount => _nb_thread_in_progress;

    /// <summary>
    /// Reinitialise le compteur partage.
    /// </summary>
    public static void ResetCounter()
    {
        _nb_thread_in_progress = 0;
    }

    /// <inheritdoc />
    public void Execute(object? threadState)
    {
        FctA(threadState);
    }

    /// <summary>
    /// Methode executee par tous les threads de la partie 1.
    /// </summary>
    /// <param name="threadState">Nom logique du thread.</param>
    public static void FctA(object? threadState)
    {
        var threadName = threadState as string ?? "Thread_Unknown";

        ++_nb_thread_in_progress;
        Console.WriteLine(
            $"[{DateTime.UtcNow:HH:mm:ss.fff}] {threadName} START - in progress: {_nb_thread_in_progress}");

        Thread.Sleep(_simulatedWorkDurationMilliseconds);

        --_nb_thread_in_progress;
        Console.WriteLine(
            $"[{DateTime.UtcNow:HH:mm:ss.fff}] {threadName} END   - in progress: {_nb_thread_in_progress}");
    }
}