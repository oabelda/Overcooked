using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioReactiveLight : MonoBehaviour
{
    public Light audioLight;
    public float sensitivity = 5f;
    public float smoothSpeed = 5f;

    private AudioSource audioSource;
    private float[] samples = new float[64];
    private float currentIntensity = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioLight == null)
            audioLight = GetComponent<Light>();
    }

    void Update()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        float sum = 0f;
        foreach (float f in samples)
            sum += f;

        float average = sum / samples.Length * sensitivity;
        currentIntensity = Mathf.Lerp(currentIntensity, average, Time.deltaTime * smoothSpeed);
        audioLight.intensity = 1f + currentIntensity;
    }
}
