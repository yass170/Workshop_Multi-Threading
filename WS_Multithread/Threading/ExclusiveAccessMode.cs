namespace WS_Multithread.Threading;

/// <summary>
/// Repertorie les primitives disponibles pour la section d'acces exclusif.
/// </summary>
internal enum ExclusiveAccessMode
{
    /// <summary>
    /// Aucun mecanisme d'acces exclusif.
    /// </summary>
    None = 0,

    /// <summary>
    /// Synchronisation via mot-cle <c>lock</c>.
    /// </summary>
    Lock = 1,

    /// <summary>
    /// Synchronisation via classe <see cref="Monitor"/>.
    /// </summary>
    Monitor = 2,

    /// <summary>
    /// Synchronisation via <see cref="Mutex"/>.
    /// </summary>
    Mutex = 3,

    /// <summary>
    /// Limitation de cohorte via <see cref="Semaphore"/>.
    /// </summary>
    Semaphore = 4
}
