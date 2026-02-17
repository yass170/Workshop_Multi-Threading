namespace WS_Multithread.Threading;

/// <summary>
/// Definit une primitive capable de controler l'acces a une section critique.
/// </summary>
internal interface IExclusiveAccessPrimitive : IDisposable
{
    /// <summary>
    /// Obtient le nom lisible de la primitive.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Tente d'executer une section critique avec la strategie de synchronisation courante.
    /// </summary>
    /// <param name="criticalSection">Code de la section critique.</param>
    /// <param name="timeout">Temps maximal d'attente avant abandon.</param>
    /// <returns><see langword="true"/> si la section critique a ete executee; sinon <see langword="false"/>.</returns>
    bool TryExecuteExclusive(Action criticalSection, TimeSpan timeout);
}
