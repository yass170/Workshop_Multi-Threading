namespace WS_Multithread.Threading;

/// <summary>
/// Porte la configuration de lancement des threads.
/// </summary>
internal sealed class ThreadBatchOptions
{
    /// <summary>
    /// Initialise une nouvelle instance de <see cref="ThreadBatchOptions"/>.
    /// </summary>
    /// <param name="threadCount">Nombre total de threads a lancer.</param>
    /// <param name="delayBetweenStartsMilliseconds">Delai entre deux demarrages de thread.</param>
    /// <param name="simulatedWorkDurationMilliseconds">Delai de travail simule dans FctA.</param>
    public ThreadBatchOptions(
        int threadCount,
        int delayBetweenStartsMilliseconds,
        int simulatedWorkDurationMilliseconds)
    {
        if (threadCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(threadCount), "Le nombre de threads doit etre positif.");
        }

        if (delayBetweenStartsMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(delayBetweenStartsMilliseconds),
                "Le delai entre demarrages ne peut pas etre negatif.");
        }

        if (simulatedWorkDurationMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(simulatedWorkDurationMilliseconds),
                "Le delai de travail simule ne peut pas etre negatif.");
        }

        ThreadCount = threadCount;
        DelayBetweenStartsMilliseconds = delayBetweenStartsMilliseconds;
        SimulatedWorkDurationMilliseconds = simulatedWorkDurationMilliseconds;
    }

    /// <summary>
    /// Obtient le nombre total de threads a lancer.
    /// </summary>
    public int ThreadCount { get; }

    /// <summary>
    /// Obtient le delai en millisecondes entre deux demarrages de thread.
    /// </summary>
    public int DelayBetweenStartsMilliseconds { get; }

    /// <summary>
    /// Obtient le delai de travail simule dans FctA.
    /// </summary>
    public int SimulatedWorkDurationMilliseconds { get; }
}