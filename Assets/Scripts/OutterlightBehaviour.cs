using System.Collections;
using UnityEngine;

public class OutterlightBehaviour : MonoBehaviour
{
    Light mainLight;
    new AudioSource audio;

    [SerializeField] float secondsTillFlick;
    [SerializeField] int flickCount;
    [SerializeField] float timeBetweenFlick;
    [SerializeField] float flickShutDownDuration;

    [SerializeField] bool flick;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainLight = this.gameObject.GetComponent<Light>();
        audio = this.gameObject.GetComponent<AudioSource>();
        StartCoroutine(LightFlick());
    }

    IEnumerator LightFlick()
    {
        yield return new WaitForSeconds(secondsTillFlick);

        int flicksDone;

        while (flick)
        {
            flicksDone = 0;

            audio.Play();

            while (flicksDone < flickCount)
            {
                mainLight.enabled = false;
                yield return new WaitForSeconds(flickShutDownDuration);
                mainLight.enabled = true;
                flicksDone = flicksDone + 1;
                yield return new WaitForSeconds(timeBetweenFlick);
            }

            float randomNum;
            randomNum = Random.Range(10, 30);
            yield return new WaitForSeconds(randomNum);
        }
    }
}
