namespace WS_Multithread.Threading;

/// <summary>
/// N'applique aucune synchronisation sur la section critique.
/// </summary>
internal sealed class NoSynchronizationExclusiveAccessPrimitive : IExclusiveAccessPrimitive
{
    /// <inheritdoc />
    public string Name => "none";

    /// <inheritdoc />
    public void ExecuteExclusive(Action criticalSection)
    {
        if (criticalSection is null)
        {
            throw new ArgumentNullException(nameof(criticalSection));
        }

        criticalSection();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Aucun objet natif a liberer.
    }
}
