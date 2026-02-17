namespace WS_Multithread.Threading;

/// <summary>
/// Limite l'acces a la section critique a un nombre maximal de threads.
/// </summary>
internal sealed class SemaphoreCohortAccessPrimitive : IExclusiveAccessPrimitive
{
    private readonly Semaphore _semaphore;
    private readonly int _maxConcurrentUsers;

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="SemaphoreCohortAccessPrimitive"/>.
    /// </summary>
    /// <param name="semaphoreName">Nom du semaphore partage entre processus.</param>
    /// <param name="maxConcurrentUsers">Nombre maximal d'utilisateurs simultanes.</param>
    public SemaphoreCohortAccessPrimitive(string semaphoreName, int maxConcurrentUsers)
    {
        if (string.IsNullOrWhiteSpace(semaphoreName))
        {
            throw new ArgumentException("Le nom du semaphore est obligatoire.", nameof(semaphoreName));
        }

        if (maxConcurrentUsers <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxConcurrentUsers),
                "Le nombre maximal d'utilisateurs simultanes doit etre strictement positif.");
        }

        _maxConcurrentUsers = maxConcurrentUsers;
        _semaphore = new Semaphore(maxConcurrentUsers, maxConcurrentUsers, semaphoreName);
    }

    /// <inheritdoc />
    public string Name => $"semaphore({_maxConcurrentUsers})";

    /// <inheritdoc />
    public bool TryExecuteExclusive(Action criticalSection, TimeSpan timeout)
    {
        if (criticalSection is null)
        {
            throw new ArgumentNullException(nameof(criticalSection));
        }

        var hasEntered = false;

        try
        {
            hasEntered = _semaphore.WaitOne(timeout);

            if (!hasEntered)
            {
                return false;
            }

            criticalSection();
            return true;
        }
        finally
        {
            if (hasEntered)
            {
                _semaphore.Release();
            }
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _semaphore.Dispose();
    }
}
