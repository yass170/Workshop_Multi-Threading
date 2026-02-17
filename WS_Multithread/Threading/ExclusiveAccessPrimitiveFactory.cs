namespace WS_Multithread.Threading;

/// <summary>
/// Fabrique de primitives de synchronisation pour la section d'acces exclusif.
/// </summary>
internal static class ExclusiveAccessPrimitiveFactory
{
    private const string CrossProcessMutexName = @"Global\WS_IPC_ExclusiveAccess_Mutex";

    /// <summary>
    /// Cree la primitive de synchronisation correspondant au mode demande.
    /// </summary>
    /// <param name="mode">Mode d'acces exclusif.</param>
    /// <returns>Primitive de synchronisation.</returns>
    public static IExclusiveAccessPrimitive Create(ExclusiveAccessMode mode)
    {
        return mode switch
        {
            ExclusiveAccessMode.None => new NoSynchronizationExclusiveAccessPrimitive(),
            ExclusiveAccessMode.Lock => new LockExclusiveAccessPrimitive(),
            ExclusiveAccessMode.Monitor => new MonitorExclusiveAccessPrimitive(),
            ExclusiveAccessMode.Mutex => new MutexExclusiveAccessPrimitive(CrossProcessMutexName),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Mode d'acces exclusif non supporte.")
        };
    }
}
