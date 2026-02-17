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
    public bool TryExecuteExclusive(Action criticalSection, TimeSpan timeout)
    {
        if (criticalSection is null)
        {
            throw new ArgumentNullException(nameof(criticalSection));
        }

        var lockTaken = false;

        try
        {
            Monitor.TryEnter(_syncRoot, timeout, ref lockTaken);

            if (!lockTaken)
            {
                return false;
            }

            criticalSection();
            return true;
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
