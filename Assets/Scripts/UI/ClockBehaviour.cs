using UnityEngine;
using UnityEngine.UI;

public class ClockBehaviour : MonoBehaviour
{
    Text textComponent;

    void Start()
    {
        textComponent = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = GameManagerBehaviour.GetRemainingLevelTime();

        int totalSec = Mathf.RoundToInt(time);

        int min = totalSec / 60;
        int sec = totalSec % 60;

        string timeText = 
            ( min == 0? "" : string.Format("{0:0}:", min) ) 
            + string.Format("{0:00}" , sec);

        textComponent.text = timeText;
    }
}
