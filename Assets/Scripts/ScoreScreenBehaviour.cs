using UnityEngine;
using UnityEngine.UI;

public class ScoreScreenBehaviour : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text highestComboText;
    [SerializeField] Text succedText;
    [SerializeField] Text failsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.SetActive(false);
        GameManagerBehaviour.RegisterOnLevelEnded(ShowScreen);
    }

    private void ShowScreen()
    {
        this.gameObject.SetActive(true);

        highestComboText.text = GameManagerBehaviour.GetScoreManagerBehaviour().GetMaxCombo().ToString();
        scoreText.text = GameManagerBehaviour.GetScoreManagerBehaviour().GetScore().ToString();
        succedText.text = GameManagerBehaviour.GetScoreManagerBehaviour().GetDelivers().ToString();
        failsText.text = GameManagerBehaviour.GetScoreManagerBehaviour().GetFails().ToString();
    }
}
