# Workshop Multi-Threading

Repo de workshop C# sur les problemes classiques de programmation multithread:
- race conditions,
- acces exclusif,
- cohorte (N acces simultanes),
- timeout d'attente,
- synchronisation de fin d'execution.

Solution:
- `WS_IPC.sln`
- projet console: `WS_Multithread`

## Structure rapide
- `WS_Multithread/Program.cs`: point d'entree et orchestration.
- `WS_Multithread/Threading/ObservationScenario.cs`: logique metier du sujet.
- `WS_Multithread/Threading/*ExclusiveAccessPrimitive*.cs`: primitives de synchronisation.
- `WS_Multithread/Threading/ThreadBatchRunner.cs`: lancement des threads.
- `WS_Multithread/Threading/CountdownSignalingScenarioDecorator.cs`: signalement de fin via `CountdownEvent`.

## Comment utiliser le projet
Prerequis:
- .NET SDK 8+

Compilation:
```bash
dotnet build WS_IPC.sln
```

Execution (mode par defaut: `semaphore`):
```bash
dotnet run --project WS_Multithread/WS_Multithread.csproj
```

Execution avec mode explicite:
```bash
dotnet run --project WS_Multithread/WS_Multithread.csproj -- --mode semaphore
dotnet run --project WS_Multithread/WS_Multithread.csproj -- --mode mutex
dotnet run --project WS_Multithread/WS_Multithread.csproj -- --mode lock
dotnet run --project WS_Multithread/WS_Multithread.csproj -- --mode monitor
dotnet run --project WS_Multithread/WS_Multithread.csproj -- --mode none
```

Sortie utile:
- `_nb_thread_in_progress`: threads actifs en cours.
- `_CountExclusive_access`: threads dans la section controlee.
- `max _CountExclusive_access`: pic de concurrence observe.
- `threads impatients`: threads ayant abandonne apres timeout (> 200 ms).

## Reponses aux questions du sujet

### Partie 1 - Question: "Que constatez-vous ? Avez-vous une explication ?"
Constat: l'ordre d'execution des threads est non deterministe. Sans protection atomique, la valeur finale de `_nb_thread_in_progress` peut etre incoherente (pas toujours 0) a cause de courses critiques sur increment/decrement.

Explication: plusieurs threads lisent/modifient/ecrivent la meme variable en concurrence. Les operations ne sont pas atomiques sans primitive adaptee.

PR associee: [#PR-F01](https://github.com/yass170/Workshop_Multi-Threading/pull/2) (observation), [`#PR-F02`](https://github.com/yass170/Workshop_Multi-Threading/pull/2) (explication/correction avec `Interlocked`).

### Partie 1 - Question: "Quelle est la difference entre `++_nb_thread_in_progress` et `_nb_thread_in_progress++` ?"
- `++x` (prefixe): incremente puis retourne la valeur incrementee.
- `x++` (postfixe): retourne d'abord l'ancienne valeur puis incremente.

Dans les deux cas, en multithread sans synchronisation, ce n'est pas thread-safe.

PR associee: `#PR-F01`.

### Partie 1 - Question: "Etes-vous capable de definir precisement l'instruction `Sleep(30)` ?"
`Sleep(30)` demande au scheduler de suspendre le thread courant pendant environ 30 ms minimum. Ce n'est pas une garantie exacte: la reprise depend de la resolution d'horloge, de la charge machine et de l'ordonnancement, donc la duree reelle peut etre superieure.

PR associee: `#PR-F01`.

### Partie 3 - Question: "Vous devez constater une difference avec Mutex, avez-vous une explication ?"
Oui. `lock`/`Monitor` synchronisent uniquement les threads d'un meme processus. `Mutex` nomme est un objet noyau partageable entre processus: deux instances de l'application se bloquent mutuellement dessus.

PR associee: `#PR-F03`.

### Partie 3 - Question: "Dans le code de la nouvelle fonction, est-il encore necessaire d'utiliser Interlocked ?"
En acces strictement exclusif (1 seul thread a la fois), `Interlocked` n'est pas strictement necessaire pour le compteur de la section critique.  
Par contre, il reste pertinent pour rendre le code robuste si la politique change (ex: cohorte a 3 avec semaphore), et pour garder un comptage fiable quel que soit le mode.

PR associee: `#PR-F03`, `#PR-F04`.

### Partie 4 - Question: "Est-il possible de gerer un probleme de cohorte sur des processus distincts ?"
Oui, avec une primitive nommee partagee entre processus (ex: `Semaphore` nomme). Dans ce repo, la cohorte globale reste limitee a 3 meme avec plusieurs instances lancees en parallele.

PR associee: `#PR-F04`.
