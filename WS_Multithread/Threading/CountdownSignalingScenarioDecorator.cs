namespace WS_Multithread.Threading;

/// <summary>
/// Signale un <see cref="CountdownEvent"/> a la fin de chaque execution de thread.
/// </summary>
internal sealed class CountdownSignalingScenarioDecorator : IThreadScenario
{
    private readonly IThreadScenario _innerScenario;
    private readonly CountdownEvent _completionEvent;

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="CountdownSignalingScenarioDecorator"/>.
    /// </summary>
    /// <param name="innerScenario">Scenario metier execute par le thread.</param>
    /// <param name="completionEvent">Compteur de synchronisation a decremeter en fin de traitement.</param>
    public CountdownSignalingScenarioDecorator(IThreadScenario innerScenario, CountdownEvent completionEvent)
    {
        _innerScenario = innerScenario ?? throw new ArgumentNullException(nameof(innerScenario));
        _completionEvent = completionEvent ?? throw new ArgumentNullException(nameof(completionEvent));
    }

    /// <inheritdoc />
    public void Execute(object? threadState)
    {
        try
        {
            _innerScenario.Execute(threadState);
        }
        finally
        {
            _completionEvent.Signal();
        }
    }
}
