namespace WS_Multithread.Threading;

/// <summary>
/// Parse les arguments de ligne de commande pour choisir la primitive d'acces exclusif.
/// </summary>
internal static class ExclusiveAccessModeParser
{
    /// <summary>
    /// Extrait le mode d'acces exclusif depuis la ligne de commande.
    /// </summary>
    /// <param name="args">Arguments du programme.</param>
    /// <returns>Mode d'acces exclusif selectionne.</returns>
    /// <exception cref="ArgumentException">L'argument <c>--mode</c> est invalide.</exception>
    public static ExclusiveAccessMode Parse(string[] args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];

            if (argument.StartsWith("--mode=", StringComparison.OrdinalIgnoreCase))
            {
                var modeValue = argument["--mode=".Length..];
                return ParseModeValue(modeValue);
            }

            if (string.Equals(argument, "--mode", StringComparison.OrdinalIgnoreCase))
            {
                if (index + 1 >= args.Length)
                {
                    throw new ArgumentException("L'argument --mode requiert une valeur.");
                }

                return ParseModeValue(args[index + 1]);
            }
        }

        return ExclusiveAccessMode.Semaphore;
    }

    private static ExclusiveAccessMode ParseModeValue(string modeValue)
    {
        var normalizedMode = modeValue.Trim().ToLowerInvariant();

        return normalizedMode switch
        {
            "none" => ExclusiveAccessMode.None,
            "lock" => ExclusiveAccessMode.Lock,
            "monitor" => ExclusiveAccessMode.Monitor,
            "mutex" => ExclusiveAccessMode.Mutex,
            "semaphore" => ExclusiveAccessMode.Semaphore,
            _ => throw new ArgumentException(
                $"Mode d'acces invalide: '{modeValue}'. Valeurs attendues: none, lock, monitor, mutex, semaphore.")
        };
    }
}
