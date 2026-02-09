using System.Xml.Serialization;
using UnityEngine;

// añade un AudioSource con un zumbido eléctrico o chispa y actívalo/desactívalo igual que la luz.
public class BrokenLight : MonoBehaviour
{
    public Light targetLight;
    public float minWait = 0.05f;
    public float maxWait = 0.3f;

    void OnEnable()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        StartCoroutine(Glitch());
    }

    System.Collections.IEnumerator Glitch()
    {
        while (true)
        {
            targetLight.enabled = !targetLight.enabled;
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
