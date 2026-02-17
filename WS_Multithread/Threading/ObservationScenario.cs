namespace WS_Multithread.Threading;

/// <summary>
/// Implemente la partie Observation et acces exclusif du sujet.
/// </summary>
internal sealed class ObservationScenario : IThreadScenario
{
    /// <summary>
    /// Compteur partage de threads en cours.
    /// </summary>
    private static int _nb_thread_in_progress = 0;

    /// <summary>
    /// Compteur de threads presents dans la section d'acces exclusif.
    /// </summary>
    private static int _CountExclusive_access = 0;
    private static int _MaxCountExclusive_access = 0;

    private static int _simulatedWorkDurationMilliseconds = 30;
    private static IExclusiveAccessPrimitive? _exclusiveAccessPrimitive;

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="ObservationScenario"/>.
    /// </summary>
    /// <param name="simulatedWorkDurationMilliseconds">Delai simule dans la methode FctA.</param>
    /// <param name="exclusiveAccessPrimitive">Primitive de synchronisation a utiliser pour l'acces exclusif.</param>
    public ObservationScenario(
        int simulatedWorkDurationMilliseconds,
        IExclusiveAccessPrimitive exclusiveAccessPrimitive)
    {
        if (simulatedWorkDurationMilliseconds < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(simulatedWorkDurationMilliseconds),
                "Le delai de travail simule ne peut pas etre negatif.");
        }

        _exclusiveAccessPrimitive = exclusiveAccessPrimitive
            ?? throw new ArgumentNullException(nameof(exclusiveAccessPrimitive));
        _simulatedWorkDurationMilliseconds = simulatedWorkDurationMilliseconds;
    }

    /// <summary>
    /// Obtient la valeur courante du compteur partage.
    /// </summary>
    public static int CurrentInProgressCount => Interlocked.CompareExchange(ref _nb_thread_in_progress, 0, 0);

    /// <summary>
    /// Obtient la valeur courante du compteur d'acces exclusif.
    /// </summary>
    public static int CurrentExclusiveAccessCount => Interlocked.CompareExchange(ref _CountExclusive_access, 0, 0);

    /// <summary>
    /// Obtient le pic d'acces simultanes observe dans la section critique.
    /// </summary>
    public static int CurrentMaxExclusiveAccessCount => Interlocked.CompareExchange(ref _MaxCountExclusive_access, 0, 0);

    /// <summary>
    /// Reinitialise le compteur partage.
    /// </summary>
    public static void ResetCounter()
    {
        Interlocked.Exchange(ref _nb_thread_in_progress, 0);
    }

    /// <summary>
    /// Reinitialise le compteur de la section d'acces exclusif.
    /// </summary>
    public static void ResetExclusiveAccessCounter()
    {
        Interlocked.Exchange(ref _CountExclusive_access, 0);
        Interlocked.Exchange(ref _MaxCountExclusive_access, 0);
    }

    /// <inheritdoc />
    public void Execute(object? threadState)
    {
        FctA(threadState);
    }

    /// <summary>
    /// Methode executee par tous les threads.
    /// Les incrementations/decrementations sont atomiques via <see cref="Interlocked"/>.
    /// </summary>
    /// <param name="threadState">Nom logique du thread.</param>
    public static void FctA(object? threadState)
    {
        var threadName = threadState as string ?? "Thread_Unknown";

        var countAfterIncrement = Interlocked.Increment(ref _nb_thread_in_progress);
        Console.WriteLine(
            $"[{DateTime.UtcNow:HH:mm:ss.fff}] P{Environment.ProcessId} {threadName} START - in progress: {countAfterIncrement}");

        Fct_Exclusive_access(threadName);

        var countAfterDecrement = Interlocked.Decrement(ref _nb_thread_in_progress);
        Console.WriteLine(
            $"[{DateTime.UtcNow:HH:mm:ss.fff}] P{Environment.ProcessId} {threadName} END   - in progress: {countAfterDecrement}");
    }

    /// <summary>
    /// Section de code avec acces controle.
    /// </summary>
    /// <param name="threadName">Nom logique du thread.</param>
    public static void Fct_Exclusive_access(string threadName)
    {
        var exclusiveAccessPrimitive = _exclusiveAccessPrimitive
            ?? throw new InvalidOperationException("La primitive d'acces exclusif n'est pas configuree.");

        exclusiveAccessPrimitive.ExecuteExclusive(
            () =>
            {
                var countAfterEnter = Interlocked.Increment(ref _CountExclusive_access);
                UpdateMaxExclusiveAccessCount(countAfterEnter);
                Console.WriteLine(
                    $"[{DateTime.UtcNow:HH:mm:ss.fff}] P{Environment.ProcessId} {threadName} ENTER - exclusive count: {countAfterEnter}");

                try
                {
                    Thread.Sleep(_simulatedWorkDurationMilliseconds);
                }
                finally
                {
                    var countAfterExit = Interlocked.Decrement(ref _CountExclusive_access);
                    Console.WriteLine(
                        $"[{DateTime.UtcNow:HH:mm:ss.fff}] P{Environment.ProcessId} {threadName} EXIT  - exclusive count: {countAfterExit}");
                }
            });
    }

    private static void UpdateMaxExclusiveAccessCount(int candidate)
    {
        while (true)
        {
            var currentMax = CurrentMaxExclusiveAccessCount;

            if (candidate <= currentMax)
            {
                return;
            }

            var previousValue = Interlocked.CompareExchange(
                ref _MaxCountExclusive_access,
                candidate,
                currentMax);

            if (previousValue == currentMax)
            {
                return;
            }
        }
    }
}
