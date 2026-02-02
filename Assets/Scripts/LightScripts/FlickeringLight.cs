using UnityEngine;

// Crea una luz Point Light o Spot Light y asígnale este script
// Consejo: usa un color naranja, rojo oscuro o verde enfermo para un efecto más tétrico.
public class FlickeringLight : MonoBehaviour
{
    Light flickerLight;
    [SerializeField] float minIntensity = 0.5f;
    [SerializeField] float maxIntensity = 1.5f;
    [SerializeField] float flickerSpeed = 0.1f;

    private float targetIntensity;

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        targetIntensity = flickerLight.intensity;
        StartCoroutine(Flicker());
    }

    System.Collections.IEnumerator Flicker()
    {
        while (true)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            flickerLight.intensity = Mathf.Lerp(flickerLight.intensity, targetIntensity, Time.deltaTime * 10f);
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
