using UnityEngine;

public class HeartbeatLight : MonoBehaviour
{
    public Light pulseLight;
    public float baseIntensity = 0.8f;
    public float pulseStrength = 0.5f;
    public float pulseSpeed = 2f;

    void Start()
    {
        if (pulseLight == null)
            pulseLight = GetComponent<Light>();
    }

    void Update()
    {
        float pulse = Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed)) * pulseStrength;
        pulseLight.intensity = baseIntensity + pulse;
    }
}
