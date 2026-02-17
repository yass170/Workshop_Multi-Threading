namespace WS_Multithread.Threading;

/// <summary>
/// Implante l'acces exclusif avec <see cref="Monitor"/>.
/// </summary>
internal sealed class MonitorExclusiveAccessPrimitive : IExclusiveAccessPrimitive
{
    private readonly object _syncRoot = new();

    /// <inheritdoc />
    public string Name => "monitor";

    /// <inheritdoc />
    public void ExecuteExclusive(Action criticalSection)
    {
        if (criticalSection is null)
        {
            throw new ArgumentNullException(nameof(criticalSection));
        }

        var lockTaken = false;

        try
        {
            Monitor.Enter(_syncRoot, ref lockTaken);
            criticalSection();
        }
        finally
        {
            if (lockTaken)
            {
                Monitor.Exit(_syncRoot);
            }
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Aucun objet natif a liberer.
    }
}
