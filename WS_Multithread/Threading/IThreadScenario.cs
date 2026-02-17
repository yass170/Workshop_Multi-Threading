namespace WS_Multithread.Threading;

/// <summary>
/// Decrit le traitement execute par chaque thread.
/// </summary>
internal interface IThreadScenario
{
    /// <summary>
    /// Execute le scenario de thread.
    /// </summary>
    /// <param name="threadState">Etat transmis au thread lors du demarrage.</param>
    void Execute(object? threadState);
}