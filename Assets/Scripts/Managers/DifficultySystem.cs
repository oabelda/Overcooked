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

    DifficultyState currentState;

    // DEBUG / override manual
    public bool overrideDifficulty = false;
    [Range(0, 1)] public float manualDifficulty = 0.5f;

    void OnEnable()
    {
        GameEvents.OnLevelStarted += HandleLevelStart;
        GameEvents.OnLevelEnded += HandleLevelEnd;
    }

    void OnDisable()
    {
        GameEvents.OnLevelStarted -= HandleLevelStart;
        GameEvents.OnLevelEnded -= HandleLevelEnd;
    }

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
        BuildDifficultyState();
        Broadcast();
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
        if (!profile.enableAdaptive || analytics == null)
            return 0f;

        float efficiency = analytics.GetEfficiency();
        float combo = analytics.GetAverageComboNormalized();
        float scoreRate = analytics.GetScorePerMinuteNormalized();

        // estimación de habilidad
        float skill =
            efficiency * 0.5f +
            combo * 0.3f +
            scoreRate * 0.2f;

        // convertir habilidad en ajuste de dificultad
        return Mathf.Lerp(-0.25f, 0.25f, skill);
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
}

[System.Serializable]
public struct DifficultyState
{
    public float difficulty01;     // valor global normalizado (0–1)

    public float orderSpawnRate;
    public float customerPatience;
    public float scoreMultiplier;
    public float pressureLevel;
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
