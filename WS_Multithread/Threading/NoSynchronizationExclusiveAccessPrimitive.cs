namespace WS_Multithread.Threading;

/// <summary>
/// N'applique aucune synchronisation sur la section critique.
/// </summary>
internal sealed class NoSynchronizationExclusiveAccessPrimitive : IExclusiveAccessPrimitive
{
    /// <inheritdoc />
    public string Name => "none";

    /// <inheritdoc />
    public bool TryExecuteExclusive(Action criticalSection, TimeSpan timeout)
    {
        if (criticalSection is null)
        {
            throw new ArgumentNullException(nameof(criticalSection));
        }

        criticalSection();
        return true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Aucun objet natif a liberer.
    }
}
