namespace WS_Multithread.Threading;

/// <summary>
/// Definit une primitive capable de serialiser l'acces a une section critique.
/// </summary>
internal interface IExclusiveAccessPrimitive : IDisposable
{
    /// <summary>
    /// Obtient le nom lisible de la primitive.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Execute une section critique avec la strategie de synchronisation courante.
    /// </summary>
    /// <param name="criticalSection">Code de la section critique.</param>
    void ExecuteExclusive(Action criticalSection);
}
