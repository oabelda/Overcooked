using UnityEngine;

// Puedes añadir este script a cualquier objeto vacío (Empty GameObject):
// el nivel parece “respirar”, como si la niebla viva se moviera.
public class CreepyFog : MonoBehaviour
{
    public Color startColor = new Color(0.1f, 0.05f, 0.1f);
    public Color endColor = new Color(0.05f, 0.02f, 0.05f);
    public float fogDensityMin = 0.01f;
    public float fogDensityMax = 0.05f;
    public float speed = 0.5f;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        RenderSettings.fogColor = Color.Lerp(startColor, endColor, t);
        RenderSettings.fogDensity = Mathf.Lerp(fogDensityMin, fogDensityMax, t);
    }
}
