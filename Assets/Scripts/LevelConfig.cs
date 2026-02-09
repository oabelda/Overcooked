using UnityEngine;

[CreateAssetMenu(menuName ="Level")]
public class LevelConfig : ScriptableObject
{
    [Header("Time")]
    [SerializeField] float levelDuration;

    [Header("Score")]
    [SerializeField] int baseDeliverScore;
    [SerializeField] int ingredientScore;

    [Header("Tips")]
    [SerializeField] int minTip;
    [SerializeField] int maxTip;

    [Header("Combo")]
    [SerializeField] int comboStep;
    [SerializeField] int maxComboMultiplier;

    [Header("Stars")]
    [SerializeField] int oneStar;
    [SerializeField] int twoStar;
    [SerializeField] int threeStar;

    public int GetBaseDeliverScore()
    {
        return baseDeliverScore;
    }

    public int GetIngredientScore()
    {
        return ingredientScore;
    }

    public int GetMinTip()
    {
        return minTip;
    }

    public int GetMaxTip()
    {
        return maxTip;
    }

    public int GetComboStep()
    {
        return comboStep;
    }

    public int GetMaxComboMultiplier()
    {
        return maxComboMultiplier;
    }

    public float GetLevelDuration()
    {
        return levelDuration;
    }

    public int GetStars(int score, int playersCount)
    {
        if (score >= threeStar * playersCount) return 3;
        if (score >= twoStar * playersCount) return 2;
        if (score >= oneStar * playersCount) return 1;
        return 0;
    }
}
