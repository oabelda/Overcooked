using UnityEngine;

[System.Serializable]
public class Pressure
{
    //[Header("Pressure")]
    [Range(0, 1)]
    [SerializeField] float pressure;
    [SerializeField] AnimationCurve pressureCuver;

    float eventModifier;
    float eventDelayMultiplier = 1;

    int consecutiveSpawns;
    float avgDelivery;
    int comboCount;

    public Pressure(float initialValue = 0)
    {
        Value = initialValue;
        comboCount = 0;
        avgDelivery = 0;
        consecutiveSpawns = 0;
        GameManagerBehaviour.OnOrderFailed += OnOrderFailed;
        GameManagerBehaviour.OnOrderServed += OnOrderServed;
    }

    private void OnOrderServed(Order order, int index)
    {
        // Update pressure
        float speedScore = order.GetRelativeSpeed(1.5f,0.5f);

        avgDelivery = Mathf.Lerp(avgDelivery, (1 - speedScore) * 2f, 0.2f);

        // Raise pressure (fixed) + (combo) + (speed) + (avgSpeed)
        Value += 0.1f + (comboCount++ * 0.02f) + (speedScore * 0.08f) + (avgDelivery * 0.04f);
    }

    public void Dispose()
    {
        GameManagerBehaviour.OnOrderFailed -= OnOrderFailed;
    }

    // Propiedad pública para acceder/modificar el valor
    private float Value
    {
        get => pressure;
        set { pressure = Mathf.Clamp01(value); }
    }

    // Sobrecarga del operador + para sumar un Pressure con un float
    public static Pressure operator +(Pressure p, float f)
    {
        return new Pressure(p.pressure + f);
    }

    // Sobrecarga del operador + para float + Pressure
    public static Pressure operator +(float f, Pressure p)
    {
        return new Pressure(p.pressure + f);
    }

    // Sobrecarga del operador - para restar un Pressure con un float
    public static Pressure operator -(Pressure p, float f)
    {
        return new Pressure(p.pressure - f);
    }

    // Sobrecarga del operador - para float - Pressure
    public static Pressure operator -(float f, Pressure p)
    {
        return new Pressure(p.pressure - f);
    }

    // Conversión implícita a float (para usarlo en expresiones)
    public static implicit operator float(Pressure p)
    {
        return p.pressure;
    }

    // Conversión implícita desde float (para poder asignar un float directamente)
    public static implicit operator Pressure(float f)
    {
        return new Pressure(f);
    }

    public void OnOrderFailed(Order order, int index)
    {
        // Pressure Update On Order Failed
        comboCount = 0;
        Value -= 0.15f;
    }

    public float GetEffectivePressure()
    {
        return pressureCuver.Evaluate(Value) + eventModifier;
    }

    public bool PressureTry(float minChance, float maxChance, int consecutiveSpawnsOffset, float consecutiveSpanwsMultiplier)
    {
        // Get effective pressure
        float effectivePressure = GetEffectivePressure();

        float chance = Mathf.Lerp(minChance, maxChance, effectivePressure);

        if (consecutiveSpawns >= consecutiveSpawnsOffset)
            chance *= consecutiveSpanwsMultiplier;

        if (Random.value < chance)
        {
            ++consecutiveSpawns;
            return true;
        }
        else
        {
            consecutiveSpawns = 0;
            return false;
        }
    }

    public float PressureLerp(float max, float min)
    {
        return Mathf.Lerp(max, min, GetEffectivePressure()) * eventDelayMultiplier;
    }

    #region Pressure Events
    //public void TriggerRush(int seconds, float eventModifier)
    //{
    //    StartCoroutine(RushEvent(seconds, eventModifier));
    //}

    //private IEnumerator RushEvent(int seconds, float eventModifier)
    //{
    //    this.eventModifier += eventModifier;
    //    yield return new WaitForSeconds(seconds);
    //    this.eventModifier -= eventModifier;
    //}

    //public void TriggerCalm(int seconds, float delayMultiplier)
    //{
    //    StartCoroutine(CalmEvent(seconds, delayMultiplier));
    //}

    //private IEnumerator CalmEvent(int seconds, float delayMultiplier)
    //{
    //    this.eventDelayMultiplier += delayMultiplier;
    //    yield return new WaitForSeconds(seconds);
    //    this.eventDelayMultiplier -= delayMultiplier;
    //}
    #endregion

    public override string ToString()
    {
        return pressure.ToString();
    }
}
