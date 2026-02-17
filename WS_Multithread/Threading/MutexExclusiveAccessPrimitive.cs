namespace WS_Multithread.Threading;

/// <summary>
/// Implante l'acces exclusif avec un <see cref="Mutex"/> nomme.
/// </summary>
internal sealed class MutexExclusiveAccessPrimitive : IExclusiveAccessPrimitive
{
    private readonly Mutex _mutex;

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="MutexExclusiveAccessPrimitive"/>.
    /// </summary>
    /// <param name="mutexName">Nom du mutex partage entre processus.</param>
    public MutexExclusiveAccessPrimitive(string mutexName)
    {
        if (string.IsNullOrWhiteSpace(mutexName))
        {
            throw new ArgumentException("Le nom du mutex est obligatoire.", nameof(mutexName));
        }

        _mutex = new Mutex(initiallyOwned: false, name: mutexName);
    }

    /// <inheritdoc />
    public string Name => "mutex";

    /// <inheritdoc />
    public void ExecuteExclusive(Action criticalSection)
    {
        if (criticalSection is null)
        {
            throw new ArgumentNullException(nameof(criticalSection));
        }

        var ownsMutex = false;

        try
        {
            try
            {
                ownsMutex = _mutex.WaitOne();
            }
            catch (AbandonedMutexException)
            {
                // Le mutex est considere acquis par ce thread.
                ownsMutex = true;
            }

            criticalSection();
        }
        finally
        {
            if (ownsMutex)
            {
                _mutex.ReleaseMutex();
            }
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _mutex.Dispose();
    }
}
