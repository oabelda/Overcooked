using UnityEngine;

public class ColorCycleLight : MonoBehaviour
{
    public Light magicLight;
    public float speed = 0.3f;
    public Color[] colors;

    private int currentIndex = 0;
    private int nextIndex = 1;
    private float t = 0f;

    void Start()
    {
        if (magicLight == null)
            magicLight = GetComponent<Light>();
        if (colors.Length < 2)
            colors = new Color[] { Color.red, Color.green, Color.blue };
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        magicLight.color = Color.Lerp(colors[currentIndex], colors[nextIndex], t);

        if (t >= 1f)
        {
            t = 0f;
            currentIndex = nextIndex;
            nextIndex = (nextIndex + 1) % colors.Length;
        }
    }
}
