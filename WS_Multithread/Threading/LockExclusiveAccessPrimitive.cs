namespace WS_Multithread.Threading;

/// <summary>
/// Implante l'acces exclusif avec le mot-cle <c>lock</c>.
/// </summary>
internal sealed class LockExclusiveAccessPrimitive : IExclusiveAccessPrimitive
{
    private readonly object _syncRoot = new();

    /// <inheritdoc />
    public string Name => "lock";

    /// <inheritdoc />
    public void ExecuteExclusive(Action criticalSection)
    {
        if (criticalSection is null)
        {
            throw new ArgumentNullException(nameof(criticalSection));
        }

        lock (_syncRoot)
        {
            criticalSection();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Aucun objet natif a liberer.
    }
}
