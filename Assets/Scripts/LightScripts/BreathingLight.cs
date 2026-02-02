using UnityEngine;

public class BreathingLight : MonoBehaviour
{
    public Light eerieLight;
    public float intensityVariation = 0.3f;
    public float speed = 0.5f;
    private float baseIntensity;

    void Start()
    {
        if (eerieLight == null)
            eerieLight = GetComponent<Light>();
        baseIntensity = eerieLight.intensity;
    }

    void Update()
    {
        eerieLight.intensity = baseIntensity + Mathf.Sin(Time.time * speed) * intensityVariation;
    }
}
