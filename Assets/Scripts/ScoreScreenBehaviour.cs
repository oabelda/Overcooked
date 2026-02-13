using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreenBehaviour : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text highestComboText;
    [SerializeField] Text succedText;
    [SerializeField] Text failsText;
    [SerializeField] Text scoreLabel;
    [SerializeField] Text highestComboLabel;
    [SerializeField] Text succedLabel;
    [SerializeField] Text failsLabel;
    [SerializeField] Image[] stars;

    [SerializeField] float timeBetweenShows;

    [SerializeField] Color lightOffStars;
    [SerializeField] Color lightOnStars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.SetActive(false);
        GameManagerBehaviour.RegisterOnLevelEnded(ShowScreen);
    }

    private void ShowScreen()
    {
        HideAll();

        this.gameObject.SetActive(true);

        StartCoroutine(ShowResults());
    }

    IEnumerator ShowResults()
    {
        int maxCombo = GameManagerBehaviour.GetScoreManagerBehaviour().GetMaxCombo();
        int score = GameManagerBehaviour.GetScoreManagerBehaviour().GetScore();
        int succed = GameManagerBehaviour.GetScoreManagerBehaviour().GetDelivers();
        int fails = GameManagerBehaviour.GetScoreManagerBehaviour().GetFails();
        int stars = GameManagerBehaviour.GetScoreManagerBehaviour().GetStars(2);
        // @TODO Num players

        yield return new WaitForSeconds(timeBetweenShows);
        succedLabel.enabled = true;

        yield return new WaitForSeconds(timeBetweenShows);
        yield return StartCoroutine(ShowNumberAsc(succed, succedText));

        yield return new WaitForSeconds(timeBetweenShows);
        failsLabel.enabled = true;

        yield return new WaitForSeconds(timeBetweenShows);
        yield return StartCoroutine(ShowNumberAsc(fails, failsText));

        yield return new WaitForSeconds(timeBetweenShows);
        highestComboLabel.enabled = true;

        yield return new WaitForSeconds(timeBetweenShows);
        yield return StartCoroutine(ShowNumberAsc(maxCombo, highestComboText));

        yield return new WaitForSeconds(timeBetweenShows);
        scoreLabel.enabled = true;

        yield return new WaitForSeconds(timeBetweenShows);
        yield return StartCoroutine(ShowNumberAsc(score, scoreText));

        yield return new WaitForSeconds(timeBetweenShows * 2);
        for (int i = 0; i < stars; ++i)
        {
            this.stars[i].color = lightOnStars;
            yield return new WaitForSeconds(timeBetweenShows);
        }
    }

    IEnumerator ShowNumberAsc(int num, Text text)
    {
        text.text = "0";

        yield return new WaitForSeconds(timeBetweenShows);

        float timeIteration = num > 10 ? timeBetweenShows * 2 / num : timeBetweenShows / num;

        for (int count = 1; count <= num; ++count) 
        {
            text.text = count.ToString();
            yield return new WaitForSeconds(timeIteration);
        }
    }

    void HideAll()
    {
        scoreText.text = "";
        highestComboText.text = "";
        succedText.text = "";
        failsText.text = "";
        scoreLabel.enabled = false;
        highestComboLabel.enabled = false;
        succedLabel.enabled = false;
        failsLabel.enabled = false;
        for (int i = 0; i < stars.Length; ++i)
        {
            stars[i].color = lightOffStars;
        }
        
    }
}
