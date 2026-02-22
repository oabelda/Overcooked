using System;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySystem : MonoBehaviour
{
    [SerializeField] DifficultyProfile profile;
    [SerializeField] ScoreManagerBehaviour analytics;

    public static event System.Action<DifficultyState> OnDifficultyChanged;

    float levelDuration;
    float elapsedTime;

    float currentDifficulty;
    float targetDifficulty;

    public TeamStrategy currentStrategy;

    // DEBUG / override manual
    public bool overrideDifficulty = false;
    [Range(0, 1)] public float manualDifficulty = 0.5f;

    List<IntentData> intents = new();

    DifficultyState currentState;
    DifficultyState targetState;

    [Range(0.1f, 5f)]
    public float momentumSpeed = 1.0f;

    [SerializeField] OrderSystem orderGenerator;

    public List<PlayerMetrics> players;

    public Dictionary<int, float> playerPressure =
        new Dictionary<int, float>();

    public event Action<int> OnPlayerOverloaded;
    public event Action<int> OnPlayerUnderloaded;

    //void OnEnable()
    //{
    //    GameEvents.OnLevelStarted += HandleLevelStart;
    //    GameEvents.OnLevelEnded += HandleLevelEnd;
    //}

    //void OnDisable()
    //{
    //    GameEvents.OnLevelStarted -= HandleLevelStart;
    //    GameEvents.OnLevelEnded -= HandleLevelEnd;
    //}

    void HandleLevelStart(LevelConfig level)
    {
        levelDuration = level.GetLevelDuration();
        elapsedTime = 0f;
        currentDifficulty = 0f;
    }

    void HandleLevelEnd()
    {
        enabled = false;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        ComputeTargetDifficulty();
        SmoothDifficulty();
        EvaluatePlayers();
        DetectStrategy();
        BuildDifficultyState();
        Broadcast();
    }

    void DetectStrategy()
    {
        //float specialization =
        //    analytics.ActionVarianceBetweenPlayers();

        //float stockpile =
        //    analytics.PreparedIngredientAverage();

        //float reactive =
        //    analytics.LateOrderStartRatio();

        //if (specialization > 0.6f)
        //    currentStrategy = TeamStrategy.SpecializedRoles;

        //else if (stockpile > 0.7f)
        //    currentStrategy = TeamStrategy.Stockpiling;

        //else if (reactive > 0.6f)
        //    currentStrategy = TeamStrategy.ReactiveCooking;

        //else
        //    currentStrategy = TeamStrategy.AssemblyLine;
    }

    void EvaluatePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            float pressure = CalculatePressure(players[i]);
            playerPressure[i] = pressure;

            if (pressure > 60)
                OnPlayerOverloaded?.Invoke(i);

            else if (pressure < 20)
                OnPlayerUnderloaded?.Invoke(i);
        }
    }

    float CalculatePressure(PlayerMetrics p)
    {
        return
            p.activeTasks * 2f +
            p.recentMistakes * 3f +
            p.movementBlockedTime * 1.5f -
            p.idleTime * 2f;
    }

    // =====================================================
    // 1️⃣ DIFICULTAD BASE POR TIEMPO (Overcooked clásico)
    // =====================================================
    float ComputeTimeDifficulty()
    {
        float t = Mathf.Clamp01(elapsedTime / levelDuration);
        return profile.difficultyOverLevelTime.Evaluate(t);
        // (Match Pacing) Warmup (25%), Flow (55%), Escalation (80%), Clímax (95%), HeroFinish
    }

    // =====================================================
    // 2️⃣ DIFICULTAD ADAPTATIVA (skill estimation)
    // =====================================================
    float ComputeAdaptiveDifficulty()
    {
        //if (!profile.enableAdaptive || analytics == null)
            return 0f;

        //float efficiency = analytics.GetEfficiency();
        //float combo = analytics.GetAverageComboNormalized();
        //float scoreRate = analytics.GetScorePerMinuteNormalized();

        //// estimación de habilidad
        //float skill =
        //    efficiency * 0.5f +
        //    combo * 0.3f +
        //    scoreRate * 0.2f;

        //// convertir habilidad en ajuste de dificultad
        //return Mathf.Lerp(-0.25f, 0.25f, skill);
    }

    void AdjustBias()
    {
        //reliefBias = 0;
        //chaosBias = 0;

        //if (starExpectationIndex < 0.7f)
        //    reliefBias = 1f;

        //else if (starExpectationIndex > 1.15f)
        //    chaosBias = 1f;
    }

    // =====================================================
    // 3️⃣ Combinar todas las vías
    // =====================================================
    void ComputeTargetDifficulty()
    {
        if (overrideDifficulty)
        {
            targetDifficulty = manualDifficulty;
            return;
        }

        float baseDifficulty = ComputeTimeDifficulty();
        float adaptiveOffset = ComputeAdaptiveDifficulty();

        targetDifficulty =
            Mathf.Clamp01(
                baseDifficulty +
                adaptiveOffset * profile.adaptiveStrength
            );
    }

    // =====================================================
    // 4️⃣ Suavizado (evita saltos)
    // =====================================================
    void SmoothDifficulty()
    {
        currentDifficulty = Mathf.Lerp(
            currentDifficulty,
            targetDifficulty,
            Time.deltaTime * profile.smoothingSpeed
        );
    }

    // =====================================================
    // 5️⃣ Convertir dificultad → parámetros jugables
    // =====================================================
    void BuildDifficultyState()
    {
        float d = currentDifficulty;

        currentState = new DifficultyState
        {
            difficulty01 = d,
            orderSpawnRate = profile.spawnRateCurve.Evaluate(d),
            customerPatience = profile.patienceCurve.Evaluate(d),
            scoreMultiplier = profile.scoreMultiplierCurve.Evaluate(d),
            pressureLevel = d
        };
    }

    // =====================================================
    // 6️⃣ Emitir resultado
    // =====================================================
    void Broadcast()
    {
        OnDifficultyChanged?.Invoke(currentState);
    }

    public void RegisterIntent(KitchenIntent intent, float priority)
    {
        intents.Add(new IntentData(intent, priority));
    }

    void LateUpdate()
    {
        ResolveIntentions();
        intents.Clear();
    }

    void ResolveIntentions()
    {
        float chaos = 0;
        float relief = 0;

        foreach (var i in intents)
        {
            switch (i.intent)
            {
                case KitchenIntent.IncreaseChaos:
                    chaos += i.priority;
                    break;

                case KitchenIntent.ProvideRelief:
                    relief += i.priority;
                    break;
            }
        }

        targetState.chaosLevel =
            Mathf.Clamp01(chaos - relief * 0.5f);

        targetState.reliefLevel =
            Mathf.Clamp01(relief);
    }

    //void Update()
    //{
    //    currentState.chaosLevel = Mathf.Lerp(
    //        currentState.chaosLevel,
    //        targetState.chaosLevel,
    //        Time.deltaTime * momentumSpeed);

    //    currentState.reliefLevel = Mathf.Lerp(
    //        currentState.reliefLevel,
    //        targetState.reliefLevel,
    //        Time.deltaTime * momentumSpeed);

    //    currentState.cooperationBias = Mathf.Lerp(
    //        currentState.cooperationBias,
    //        targetState.cooperationBias,
    //        Time.deltaTime * momentumSpeed);
    //}

    public DifficultyState GetCurrentState()
    {
        return currentState;
    }

    //void Update()
    //{
    //    float pressure = EvaluatePressure();

    //    // DecideIntervention
    //    {
    //        if (funPressureIndex < 30)
    //            InjectChaos(); // CreateResourceConflict(); MixRecipeDependencies();

    //        else if (funPressureIndex > 70)
    //            CreateReliefMoment();
    //    }
    //}


    //float EvaluatePressure()
    //{
    //    float orderStress =
    //        analytics.activeOrders * 5f +
    //        analytics.ordersNearFail * 10f;

    //    float chaosStress =
    //        analytics.playerCollisions * 2f +
    //        analytics.burnedFood * 8f;

    //    float idlePenalty =
    //        analytics.timeWithoutMistakes < 20f ? -10f : 0f;

    //    return Mathf.Clamp(orderStress + chaosStress + idlePenalty, 0, 100);

    //    funPressureIndex =
    //        analytics.activeOrders * 0.4f +
    //        analytics.ordersNearFail * 1.2f +
    //        analytics.recentMistakes * 0.8f -
    //        analytics.comboChain * 0.5f;

    //    funPressureIndex = Mathf.Clamp(funPressureIndex, 0, 100);
    //}

    void HandleOverloadedPlayer(int playerId)
    {
        orderGenerator.IncreaseParallelRecipes();
        orderGenerator.ReduceCriticalDependencies();
    }

    void HandleUnderloadedPlayer(int playerId)
    {
        orderGenerator.SpawnAssistTasksNear(playerId);
    }
}

[System.Serializable]
public struct DifficultyState
{
    public float difficulty01;     // valor global normalizado (0–1)

    public float orderSpawnRate;
    public float customerPatience;
    public float scoreMultiplier;
    public float pressureLevel;
    public float chaosLevel;
    public float reliefLevel;
    public float cooperationBias;
    public float recipeComplexity;
}


[CreateAssetMenu(menuName = "DifficultyProfile")]
public class DifficultyProfile : ScriptableObject
{
    [Header("Base progression")]
    public AnimationCurve difficultyOverLevelTime;

    [Header("Spawn rate")]
    public AnimationCurve spawnRateCurve;

    [Header("Patience")]
    public AnimationCurve patienceCurve;

    [Header("Score modifier")]
    public AnimationCurve scoreMultiplierCurve;

    [Header("Adaptive Difficulty")]
    public bool enableAdaptive = true;
    public float adaptiveStrength = 0.4f;

    [Header("Smoothing")]
    public float smoothingSpeed = 1.5f;
}

public enum KitchenIntent
{
    None,
    IncreaseChaos,
    ReducePressure,
    EncourageCooperation,
    EnableClimax,
    ProvideRelief
}

public struct IntentData
{
    public KitchenIntent intent;
    public float priority;

    public IntentData(KitchenIntent intent, float priority)
    {
        this.intent = intent;
        this.priority = priority;
    }
}

public class PlayerMetrics
{
    public float activeTasks;
    public float recentMistakes;
    public float movementBlockedTime;
    public float idleTime;
    public float successfulActions;
}

public enum TeamStrategy
{
    Undefined,
    SpecializedRoles,
    AssemblyLine,
    ReactiveCooking,
    Stockpiling
}